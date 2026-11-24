using Microsoft.AspNetCore.Http;

namespace EduPro.Service.Abstracts
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(string location, IFormFile file);
        void DeleteImage(string imagePath);
    }
}
