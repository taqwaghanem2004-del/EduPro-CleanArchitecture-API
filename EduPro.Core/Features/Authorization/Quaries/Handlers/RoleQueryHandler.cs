using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Authorization.Quaries.Results;
using EduPro.Core.Features.Authorization.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Results.Authorization;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ResponseHandler,
        IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResult>>>,
        IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResult>>,
        IRequestHandler<ManageUserRolesQuery, Response<ManageUserRolesResult>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public RoleQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                IAuthorizationService authorizationService,
                                IMapper mapper) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<GetRolesListResult>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesList();
            var result = _mapper.Map<List<GetRolesListResult>>(roles);
            return Success(result);
        }

        public async Task<Response<GetRoleByIdResult>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleById(request.Id);
            if (role == null) return NotFound<GetRoleByIdResult>();
            var result = _mapper.Map<GetRoleByIdResult>(role);
            return Success(result);
        }

        public async Task<Response<ManageUserRolesResult>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _authorizationService.GetUserById(request.UserId);
            if (user == null) return NotFound<ManageUserRolesResult>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            var result = await _authorizationService.ManageUserRolesData(user);
            return Success(result);
        }
        #endregion
    }
}