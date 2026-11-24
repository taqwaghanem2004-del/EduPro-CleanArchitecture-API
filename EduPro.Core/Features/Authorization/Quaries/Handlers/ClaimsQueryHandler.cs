using EduPro.Core.Bases;
using EduPro.Core.Features.Authorization.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Results.Authorization;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Authorization.Queries.Handlers
{
    namespace SchoolProject.Core.Features.Authorization.Quaries.Handlers
    {
        public class ClaimsQueryHandler : ResponseHandler,
            IRequestHandler<ManageUserClaimsQuery, Response<ManageUserClaimsResult>>
        {
            #region Fileds
            private readonly IAuthorizationService _authorizationService;
            private readonly UserManager<User> _userManager;
            private readonly IStringLocalizer<SharedResources> _stringLocalizer;
            #endregion
            #region Constructors
            public ClaimsQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                      IAuthorizationService authorizationService,
                                      UserManager<User> userManager) : base(stringLocalizer)
            {
                _authorizationService = authorizationService;
                _userManager = userManager;
                _stringLocalizer = stringLocalizer;
            }
            #endregion
            #region Handle Functions
            public async Task<Response<ManageUserClaimsResult>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
            {
                var user = await _authorizationService.GetUserById(request.UserId);
                if (user == null) return NotFound<ManageUserClaimsResult>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
                var result = await _authorizationService.ManageUserClaimData(user);
                return Success(result);
            }
            #endregion
        }

    }
}