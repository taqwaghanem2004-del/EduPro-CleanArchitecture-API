using EduPro.Core.Bases;
using EduPro.Core.Features.Authentication.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Data.Results;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.Extensions.Localization;
using EduPro.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EduPro.Core.Features.Authentication.Commands.Handlers
{
    public class GoogleSignInCommandHandler : ResponseHandler,
        IRequestHandler<GoogleSignInCommand, Response<JwtAuthResult>>
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserActivityService _userActivityService;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public GoogleSignInCommandHandler(IAuthenticationService authenticationService,
                                          IStringLocalizer<SharedResources> localizer,
                                          IUserActivityService userActivityService,
                                          UserManager<User> userManager) : base(localizer)
        {
            _authenticationService = authenticationService;
            _localizer = localizer;
            _userActivityService = userActivityService;
            _userManager = userManager;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<JwtAuthResult>> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.GoogleSignInAsync(request.IdToken);
            if (result == null)
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.InvalidGoogleToken]);

        
            var user = await _userManager.FindByNameAsync(result.RefreshToken.UserName);
            if (user != null)
            {
                await _userActivityService.LogAsync(
                    user.Id,
                    ActivityType.GoogleSignIn,
                    $"Google sign-in: {user.Email}");
            }

            return Success(result);
        }
        #endregion
    }
}
