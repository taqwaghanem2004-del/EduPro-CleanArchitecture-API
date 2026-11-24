using EduPro.Core.Bases;
using EduPro.Core.Features.Authentication.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.Extensions.Localization;
namespace EduPro.Core.Features.Authentication.Queries.Handlers
{
    public class AuthenticationQueryHandler : ResponseHandler,
       IRequestHandler<ConfirmEmailQuery, Response<string>>,
       IRequestHandler<ConfirmResetPasswordQuery, Response<string>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public AuthenticationQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                          IAuthenticationService authenticationService) : base(stringLocalizer)
        {
            _localizer = stringLocalizer;
            _authenticationService = authenticationService;
        }
        public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ConfirmEmail(request.UserId, request.Code);
            if (result == "ErrorWhenConfirmEmail")
                return BadRequest<string>(_localizer[SharedResourcesKeys.ErrorWhenConfirmEmail]);
            return Success<string>(_localizer[SharedResourcesKeys.ConfirmEmailDone]);
        }
        public async Task<Response<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ConfirmResetPassword(request.Code, request.Email);
            switch (result)
            {
                case "UserNotFound": return BadRequest<string>(_localizer[SharedResourcesKeys.UserIsNotFound]);
                case "Failed": return BadRequest<string>(_localizer[SharedResourcesKeys.InvaildCode]);
                case "Success": return Success<string>(_localizer[SharedResourcesKeys.Success]);
                default: return BadRequest<string>(_localizer[SharedResourcesKeys.InvaildCode]);
            }
        }
    }
}