using EduPro.Data.Entities.Identity;
using EduPro.Infrastructure.Data;
using EduPro.Service.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly AppDbContext _dbContext;
        private readonly IUrlHelper _urlHelper;
        private readonly IFileService _fileService;


        public ApplicationUserService(UserManager<User> userManager,
                                      IHttpContextAccessor httpContextAccessor,
                                      IEmailsService emailsService,
                                      AppDbContext dbContext,
                                      IUrlHelper urlHelper,
                                      IFileService fileService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailsService = emailsService;
            _dbContext = dbContext;
            _urlHelper = urlHelper;
            _fileService = fileService;
        }

        public async Task<string> RegisterAsync(User user, string password)
        {
            var trans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var existUser = await _userManager.FindByEmailAsync(user.Email);
                if (existUser != null) return "EmailIsExist";

                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description));

                await _userManager.AddToRoleAsync(user, "Student");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var request = _httpContextAccessor.HttpContext!.Request;
                var returnUrl = request.Scheme + "://" + request.Host +
                                _urlHelper.Action("ConfirmEmail", "Authentication",
                                new { userId = user.Id, code = code });

                var message = $@"
<!DOCTYPE html>
<html>
<body style='margin: 0; padding: 0; background-color: #f4f4f4; font-family: Arial, sans-serif;'>
    <div style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
        
        <div style='background-color: #1a73e8; padding: 30px; text-align: center;'>
            <h1 style='color: white; margin: 0; font-size: 28px;'>EduPro 🎓</h1>
            <p style='color: #e8f0fe; margin: 5px 0 0 0; font-size: 14px;'>E-Learning Platform</p>
        </div>

        <div style='padding: 40px 30px; text-align: left;'>
            <h2 style='color: #333333;'>Welcome to EduPro, {user.FullName}! 👋</h2>
            <p style='color: #666666; font-size: 16px; line-height: 1.8;'>
                We're excited to have you join our e-learning community!<br><br>
                To get started and access all educational materials,
                please confirm your email address by clicking the button below:
            </p>

            <div style='text-align: center; margin: 35px 0;'>
                <a href='{returnUrl}' 
                   style='background-color: #1a73e8; 
                          color: white; 
                          padding: 15px 40px; 
                          text-decoration: none; 
                          border-radius: 25px;
                          font-size: 16px;
                          font-weight: bold;
                          display: inline-block;'>
                    Confirm Email ✅
                </a>
            </div>

            <p style='color: #666666; font-size: 14px;'>
                If the button doesn't work, copy and paste this link into your browser:
            </p>
            <p style='word-break: break-all;'>
                <a href='{returnUrl}' style='color: #1a73e8;'>{returnUrl}</a>
            </p>

            <p style='color: #999999; font-size: 13px; margin-top: 30px;'>
                ⚠️ If you didn't create this account, you can safely ignore this email.
            </p>
        </div>

        <div style='background-color: #f8f8f8; padding: 20px; text-align: center; border-top: 1px solid #eeeeee;'>
            <p style='color: #999999; font-size: 12px; margin: 0;'>EduPro Team 🎓 | Palestine Technical University</p>
        </div>

    </div>
</body>
</html>";

                await _emailsService.SendEmail(user.Email!, message, "Confirm Your EduPro Account");
                await trans.CommitAsync();
                return "Success";
            }
            catch
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }
        public async Task<string> UpdateProfileAsync(int userId, string? fullName, string? email, IFormFile? profilePicture)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return "NotFound";

            if (!string.IsNullOrEmpty(fullName))
                user.FullName = fullName;

            if (!string.IsNullOrEmpty(email) && email != user.Email)
            {
                var emailExists = await _userManager.FindByEmailAsync(email);
                if (emailExists != null) return "EmailIsExist";

                var token = await _userManager.GenerateChangeEmailTokenAsync(user, email);
                var request = _httpContextAccessor.HttpContext!.Request;
                var encodedToken = Uri.EscapeDataString(token);
                var encodedEmail = Uri.EscapeDataString(email);

                var confirmUrl = request.Scheme + "://" + request.Host +
                                 _urlHelper.Action("ConfirmEmailChange", "Profile",
                                 new { userId = user.Id, newEmail = email, token = encodedToken });

                var message = $@"
<!DOCTYPE html>
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <div style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 10px; overflow: hidden;'>
        <div style='background-color: #1a73e8; padding: 30px; text-align: center;'>
            <h1 style='color: white; margin: 0;'>EduPro 🎓</h1>
        </div>
        <div style='padding: 40px 30px;'>
            <h2 style='color: #333333;'>Email Change Request</h2>
            <p style='color: #666666; font-size: 16px; line-height: 1.8;'>
                Someone requested to change the email of your account to: <strong>{email}</strong><br><br>
                If this was you, click the button below to confirm:
            </p>
            <div style='text-align: center; margin: 35px 0;'>
                <a href='{confirmUrl}' style='background-color: #1a73e8; color: white; padding: 15px 40px; text-decoration: none; border-radius: 25px; font-size: 16px; font-weight: bold; display: inline-block;'>
                    Confirm Email Change ✅
                </a>
            </div>
            <p style='color: #999999; font-size: 13px; margin-top: 30px;'>
                ⚠️ If you didn't request this, please ignore this email.
            </p>
        </div>
    </div>
</body>
</html>";

                await _emailsService.SendEmail(user.Email!, message, "Confirm Email Change - EduPro");
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded) return "FailedToUpdate";
                return "ConfirmEmailSent";
            }

            if (profilePicture != null)
            {
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                    _fileService.DeleteImage(user.ProfilePicture);

                var imageUrl = await _fileService.UploadImageAsync("ProfilePictures", profilePicture);
                if (imageUrl == "FailedToUploadImage" || imageUrl == "InvalidImageFormat")
                    return imageUrl;

                user.ProfilePicture = imageUrl;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return "FailedToUpdate";
            return "Success";
        }
        public async Task<string> ConfirmEmailChangeAsync(int userId, string newEmail, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return "NotFound";
            var decodedToken = Uri.UnescapeDataString(token);
            var decodedEmail = Uri.UnescapeDataString(newEmail);

            var result = await _userManager.ChangeEmailAsync(user, decodedEmail, decodedToken);
            if (!result.Succeeded) return "FailedToChangeEmail";

            user.UserName = decodedEmail;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return "Success";
        }

    }



}