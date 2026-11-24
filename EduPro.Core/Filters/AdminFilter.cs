using EduPro.Data.Entities.Identity;
using EduPro.Service.AuthServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminFilter : IAsyncActionFilter
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<User> _userManager;
    private readonly string _claimName;

    public AdminFilter(ICurrentUserService currentUserService,
                       UserManager<User> userManager,
                       string claimName)
    {
        _currentUserService = currentUserService;
        _userManager = userManager;
        _claimName = claimName;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
        .Any(x => x is AllowAnonymousAttribute);
        if (hasAllowAnonymous)
        {
            await next();
            return;
        }
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new ObjectResult("Unauthorized") { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        var roles = await _currentUserService.GetCurrentUserRolesAsync();
        if (roles.All(x => x != "Admin"))
        {
            context.Result = new ObjectResult("Forbidden") { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }

        if (!string.IsNullOrEmpty(_claimName))
        {
            var userId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userClaims = await _userManager.GetClaimsAsync(user);

            if (!userClaims.Any(x => x.Type == _claimName && x.Value.ToLower() == "true"))
            {
                context.Result = new ObjectResult("Forbidden") { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }
        }

        await next();
    }
}