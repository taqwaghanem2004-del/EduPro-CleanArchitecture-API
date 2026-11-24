using EduPro.Data.Entities.Identity;
using EduPro.Data.Helpers;
using EduPro.Data.Results;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Service.Abstracts;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EduPro.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;
        private readonly IEmailsService _emailsService;
        private readonly AppDbContext _dbContext;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly GoogleAuthSettings _googleAuthSettings;

        public AuthenticationService(JwtSettings jwtSettings,
                                     IRefreshTokenRepository refreshTokenRepository,
                                     UserManager<User> userManager,
                                     IEmailsService emailsService,
                                     AppDbContext dbContext,
                                     IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
            _emailsService = emailsService;
            _dbContext = dbContext;
            _encryptionProvider = new GenerateEncryptionProvider("8a4dcaaec64d412380fe4b02193cd26f");
            _googleAuthSettings = googleAuthSettings.Value;
        }

        public async Task<JwtAuthResult> GetJWTToken(User user)
        {
            var (jwtToken, accessToken) = await GenerateJWTToken(user);
            var refreshToken = GetRefreshToken(user.UserName);

            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                IsUsed = true,
                IsRevoked = false,
                JwtId = jwtToken.Id,
                RefreshToken = refreshToken.TokenString,
                Token = accessToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(userRefreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private async Task<(JwtSecurityToken, string)> GenerateJWTToken(User user)
        {
            var claims = await GetClaims(user);
            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }

        private RefreshToken GetRefreshToken(string username)
        {
            return new RefreshToken
            {
                ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserName = username,
                TokenString = GenerateRefreshToken()
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
        }

        public JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException(nameof(accessToken));
            return new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        }

        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };
            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out _);
                return validator == null ? "InvalidToken" : "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
                return ("AlgorithmIsWrong", null);

            if (jwtToken.ValidTo > DateTime.UtcNow)
                return ("TokenIsNotExpired", null);

            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id))?.Value;
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                          x.RefreshToken == refreshToken &&
                                          x.UserId == int.Parse(userId));

            if (userRefreshToken == null) return ("RefreshTokenIsNotFound", null);

            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                return ("RefreshTokenIsExpired", null);
            }

            return (userId, userRefreshToken.ExpiryDate);
        }

        public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {
            var (newJwtToken, newToken) = await GenerateJWTToken(user);

            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken && x.UserId == user.Id);

            if (userRefreshToken != null)
            {
                userRefreshToken.Token = newToken;
                userRefreshToken.JwtId = newJwtToken.Id;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
            }

            return new JwtAuthResult
            {
                AccessToken = newToken,
                RefreshToken = new RefreshToken
                {
                    UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName))?.Value,
                    TokenString = refreshToken,
                    ExpireAt = (DateTime)expiryDate
                }
            };
        }
        public async Task<string> ConfirmEmail(int? userId, string? code)
        {
            if (userId == null || code == null) return "ErrorWhenConfirmEmail";
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return "ErrorWhenConfirmEmail";
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? "Success" : "ErrorWhenConfirmEmail";
        }

        public async Task<string> SendResetPasswordCode(string email)
        {
            var trans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return "UserNotFound";

                var chars = "0123456789";
                var random = new Random();
                var otp = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                user.OtpCode = otp;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded) return "ErrorInUpdateUser";

                var message = $@"
<!DOCTYPE html>
<html>
<body style='margin: 0; padding: 0; background-color: #f4f4f4; font-family: Arial, sans-serif;'>
    <div style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
        
        <div style='background-color: #1a73e8; padding: 30px; text-align: center;'>
            <h1 style='color: white; margin: 0; font-size: 28px;'>EduPro 🎓</h1>
        </div>

        <div style='padding: 40px 30px; text-align: left;'>
            <h2 style='color: #333333;'>Reset Your Password</h2>
            <p style='color: #666666; font-size: 16px; line-height: 1.5;'>
                We received a request to reset your password.<br><br>
                Use the code below to reset it:
            </p>

            <div style='text-align: center; margin: 35px 0;'>
                <div style='background-color: #f0f4ff; 
                            border: 2px dashed #1a73e8; 
                            border-radius: 10px; 
                            padding: 20px;
                            display: inline-block;'>
                    <p style='margin: 0; color: #1a73e8; font-size: 36px; font-weight: bold; letter-spacing: 8px;'>
                        {otp}
                    </p>
                </div>
            </div>

            <p style='color: #999999; font-size: 14px; text-align: center;'>
                ⚠️ This code is valid for a limited time. Do not share it with anyone.
            </p>
        </div>

        <div style='background-color: #f8f8f8; padding: 20px; text-align: center; border-top: 1px solid #eeeeee;'>
            <p style='color: #999999; font-size: 12px; margin: 0;'>The EduPro Team</p>
        </div>

    </div>
</body>
</html>";

                await _emailsService.SendEmail(user.Email!, message, "Reset Password - EduPro");
                await trans.CommitAsync();
                return "Success";
            }
            catch
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> ConfirmResetPassword(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "UserNotFound";
            return user.OtpCode == code ? "Success" : "Failed";
        }

        public async Task<string> ResetPassword(string email, string password)
        {
            var trans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null) return "UserNotFound";

                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password);

                await trans.CommitAsync();
                return "Success";
            }
            catch
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }
        public async Task<JwtAuthResult?> GoogleSignInAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleAuthSettings.ClientId }
            };

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch
            {
                return null;
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded) return null;

                await _userManager.AddToRoleAsync(user, "Student");
            }

            if (user.IsBlocked) return null;

            return await GetJWTToken(user);
        }
    }
}