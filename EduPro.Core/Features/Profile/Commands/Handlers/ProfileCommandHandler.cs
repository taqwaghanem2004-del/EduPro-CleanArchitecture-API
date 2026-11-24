using EduPro.Core.Bases;
using EduPro.Core.Features.Profile.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Profile.Commands.Handlers
{
    public class ProfileCommandHandler : ResponseHandler,
        IRequestHandler<UpdateProfileCommand, Response<string>>,
        IRequestHandler<ChangePasswordCommand, Response<string>>

    {
        #region Fields
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly IUserActivityService _userActivityService;

        #endregion

        #region Constructors
        public ProfileCommandHandler(
            IApplicationUserService applicationUserService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer, UserManager<User> userManager, IUserActivityService userActivityService) : base(localizer)
        {
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _userManager = userManager;
            _userActivityService = userActivityService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(
            UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _applicationUserService.UpdateProfileAsync(
               userId, request.FullName, request.Email, request.ProfilePicture);
            if (result == "Success" || result == "ConfirmEmailSent")
            {
                await _userActivityService.LogAsync(
                    userId,
                    ActivityType.UpdateProfile,
                    $"Updated profile for user ID: {userId}");
            }

            return result switch
            {
                "NotFound" => NotFound<string>(),
                "EmailIsExist" => BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsExist]),
                "InvalidImageFormat" => BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadFile]),
                "FailedToUploadImage" => BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadFile]),
                "FailedToUpdate" => BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]),
                "ConfirmEmailSent" => Success<string>("Confirmation email sent to your old email address"),
                _ => Success<string>(_localizer[SharedResourcesKeys.Success])
            };
        }
        public async Task<Response<string>> Handle(
    ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound<string>();

            var result = await _userManager.ChangePasswordAsync(
                user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
                return BadRequest<string>(result.Errors.FirstOrDefault()?.Description);

            await _userActivityService.LogAsync( 
                userId,
                ActivityType.ChangePassword,
                $"Changed password for user ID: {userId}");


            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }

        #endregion
    }
}