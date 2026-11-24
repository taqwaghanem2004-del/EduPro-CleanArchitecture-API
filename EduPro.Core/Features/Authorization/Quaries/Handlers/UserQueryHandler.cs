using EduPro.Core.Bases;
using EduPro.Core.Features.Authorization.Quaries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Results.Authorization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Authorization.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler,
        IRequestHandler<GetUsersListQuery, Response<List<GetUsersListResult>>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public UserQueryHandler(UserManager<User> userManager,
                                IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<GetUsersListResult>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
                users = users.Where(x => x.UserName.Contains(request.Keyword) ||
                                         x.Email.Contains(request.Keyword) ||
                                         x.FullName.Contains(request.Keyword));

            var result = new List<GetUsersListResult>();
            foreach (var user in await users.ToListAsync())
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new GetUsersListResult
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToList()
                });
            }

            return Success(result);
        }
        #endregion
    }
}