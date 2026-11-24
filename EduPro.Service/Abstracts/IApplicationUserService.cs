using EduPro.Data.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace EduPro.Service.Abstracts
{
    public interface IApplicationUserService
    {
        Task<string> RegisterAsync(User user, string password);
        Task<string> UpdateProfileAsync(int userId, string? fullName, string? email, IFormFile? profilePicture);
        Task<string> ConfirmEmailChangeAsync(int userId, string newEmail, string token);
    }
}
