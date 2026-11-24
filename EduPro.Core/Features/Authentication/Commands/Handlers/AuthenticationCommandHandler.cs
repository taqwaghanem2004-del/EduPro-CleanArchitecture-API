using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Authentication.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Enums;
using EduPro.Data.Results;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
namespace EduPro.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
        IRequestHandler<RegisterCommand, Response<string>>,
        IRequestHandler<SignInCommand, Response<JwtAuthResult>>,
        IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>,
        IRequestHandler<SendResetPasswordCommand, Response<string>>,
        IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly IUserActivityService _userActivityService;
        public AuthenticationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                            UserManager<User> userManager,
                                            SignInManager<User> signInManager,
                                            IAuthenticationService authenticationService,
                                            IApplicationUserService applicationUserService,
                                            IMapper mapper,
                                            IUserActivityService userActivityService) : base(stringLocalizer)
        {
            _localizer = stringLocalizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _userActivityService = userActivityService;
        }
        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            var result = await _applicationUserService.RegisterAsync(user, request.Password);

            switch (result)
            {
                case "EmailIsExist": return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsExist]);
                case "UserNameIsExist": return BadRequest<string>(_localizer[SharedResourcesKeys.UserNameIsExist]);
                case "Failed": return BadRequest<string>(_localizer[SharedResourcesKeys.TryToRegisterAgain]);
                case "Success": return Success<string>(_localizer[SharedResourcesKeys.RegisterSuccess]);
                    await _userActivityService.LogAsync( 
                    user.Id,
                    ActivityType.Register,
                    $"New user registered: {user.Email}");
                    return Success<string>(_localizer[SharedResourcesKeys.RegisterSuccess]);
                default: return BadRequest<string>(result);
            }
        }

        public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.EmailIsNotExist]);

            if (!user.EmailConfirmed)
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.EmailNotConfirmed]);

            if (user.IsBlocked)
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.UserIsBlocked]);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded)
                return BadRequest<JwtAuthResult>(_localizer[SharedResourcesKeys.PasswordNotCorrect]);

            var result = await _authenticationService.GetJWTToken(user);
            await _userActivityService.LogAsync(user.Id, ActivityType.Login,
             $"User logged in: {user.Email}");

            return Success(result);
        }
        public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = _authenticationService.ReadJWTToken(request.AccessToken);
            var userIdAndExpireDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);

            switch (userIdAndExpireDate)
            {
                case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResult>(_localizer[SharedResourcesKeys.AlgorithmIsWrong]);
                case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResult>(_localizer[SharedResourcesKeys.TokenIsNotExpired]);
                case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResult>(_localizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
                case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResult>(_localizer[SharedResourcesKeys.RefreshTokenIsExpired]);
            }

            var (userId, expiryDate) = userIdAndExpireDate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound<JwtAuthResult>();

            var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);
        }

        public async Task<Response<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.SendResetPasswordCode(request.Email);
            switch (result)
            {
                case "UserNotFound": return BadRequest<string>(_localizer[SharedResourcesKeys.UserIsNotFound]);
                case "Failed": return BadRequest<string>(_localizer[SharedResourcesKeys.TryAgainInAnotherTime]);
                case "Success": return Success<string>(_localizer[SharedResourcesKeys.Success]);
                default: return BadRequest<string>(_localizer[SharedResourcesKeys.TryAgainInAnotherTime]);
            }
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ResetPassword(request.Email, request.Password);
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
