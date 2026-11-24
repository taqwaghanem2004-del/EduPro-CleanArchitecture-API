using EduPro.Service.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EduPro.Service.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(string location, IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return "InvalidImageFormat";

            var path = Path.Combine(_webHostEnvironment.WebRootPath, location);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = Guid.NewGuid().ToString().Replace("-", "") + extension;
            var fullPath = Path.Combine(path, fileName);

            try
            {
                using var stream = File.Create(fullPath);
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                return $"/{location}/{fileName}";
            }
            catch
            {
                return "FailedToUploadImage";
            }
        }

        public void DeleteImage(string imagePath)
        {
            try
            {
                var fullPath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch { }
        }
    }
}