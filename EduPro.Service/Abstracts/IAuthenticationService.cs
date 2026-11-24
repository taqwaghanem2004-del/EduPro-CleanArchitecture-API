using EduPro.Data.Entities.Identity;
using EduPro.Data.Results;
using System.IdentityModel.Tokens.Jwt;

namespace EduPro.Service.Abstracts
{
    public interface IAuthenticationService
    {
        Task<JwtAuthResult> GetJWTToken(User user);
        JwtSecurityToken ReadJWTToken(string accessToken);
        Task<string> ValidateToken(string accessToken);
        Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        Task<string> ConfirmEmail(int? userId, string? code);

        Task<string> SendResetPasswordCode(string email);
        Task<string> ConfirmResetPassword(string code, string email);
        Task<string> ResetPassword(string email, string password);
        Task<JwtAuthResult?> GoogleSignInAsync(string idToken);
    }
}
