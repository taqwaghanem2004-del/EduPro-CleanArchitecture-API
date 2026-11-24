using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Service.Implementations
{
    public class EduFileService : IEduFileService
    {
        private readonly IEduFileRepository _eduFileRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EduFileService(IEduFileRepository eduFileRepository,
                              IWebHostEnvironment webHostEnvironment)
        {
            _eduFileRepository = eduFileRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<EduFile>> GetEduFilesByStatusAsync(EduFileStatus status)
        {
            return await _eduFileRepository.GetEduFilesByStatusAsync(status);
        }

        public async Task<List<EduFile>> GetEduFilesByCourseIdAsync(int courseId)
        {
            return await _eduFileRepository.GetEduFilesByCourseIdAsync(courseId);
        }

        public async Task<EduFile> GetEduFileByIdAsync(int id)
        {
            return await _eduFileRepository.GetTableAsTracking()
                .Include(x => x.UploadedBy)
                .Include(x => x.Course)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string> UploadEduFileAsync(EduFile eduFile, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();

            var documentExtensions = new[] { ".pdf", ".docx", ".pptx", ".xlsx" };
            var videoExtensions = new[] { ".mp4", ".mov", ".avi", ".mkv" };
            var allowedExtensions = documentExtensions.Concat(videoExtensions).ToArray();

            if (!allowedExtensions.Contains(extension))
                return "InvalidFileType";

            bool isVideo = videoExtensions.Contains(extension);
            long maxSize = isVideo ? 500L * 1024 * 1024 : 20L * 1024 * 1024;

            if (file.Length > maxSize)
                return isVideo ? "FileTooLargeVideo" : "FileTooLargeDocument";

            if (file.Length == 0)
                return "EmptyFile";

            var uploadsFolder = _webHostEnvironment.WebRootPath + "/uploads/edufiles/";
            var uniqueFileName = Guid.NewGuid().ToString().Replace("-", string.Empty) + extension;

            try
            {
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                using (FileStream fileStream = File.Create(uploadsFolder + uniqueFileName))
                {
                    await file.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    eduFile.FilePath = $"/uploads/edufiles/{uniqueFileName}";
                    await _eduFileRepository.AddAsync(eduFile);
                    return "Success";
                }
            }
            catch (Exception)
            {
                return "FailedToUploadFile";
            }
        }

        public async Task<string> ApproveEduFileAsync(int id)
        {
            var eduFile = await _eduFileRepository.GetByIdAsync(id);
            if (eduFile == null) return "NotFound";
            eduFile.Status = EduFileStatus.Approved;
            await _eduFileRepository.UpdateAsync(eduFile);
            return "Success";
        }

        public async Task<string> RejectEduFileAsync(int id)
        {
            var eduFile = await _eduFileRepository.GetByIdAsync(id);
            if (eduFile == null) return "NotFound";
            eduFile.Status = EduFileStatus.Rejected;
            await _eduFileRepository.UpdateAsync(eduFile);
            return "Success";
        }

        public async Task<string> DeleteEduFileAsync(int id, int userId)
        {
            var file = await _eduFileRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UploadedByUserId == userId);
            if (file == null) return "NotFound";
            if (file.IsDeleted) return "AlreadyDeleted";
            file.IsDeleted = true;
            await _eduFileRepository.UpdateAsync(file);
            return "Success";
        }

        public async Task<bool> IsEduFileExist(int id)
        {
            return await _eduFileRepository.GetTableNoTracking()
                .AnyAsync(x => x.Id == id);
        }
        public async Task<string> DownloadEduFileAsync(int id)
        {
            var eduFile = await _eduFileRepository.GetByIdAsync(id);
            if (eduFile == null) return "NotFound";
            eduFile.DownloadCount++;
            await _eduFileRepository.UpdateAsync(eduFile);
            return eduFile.FilePath;
        }
        public async Task<List<EduFile>> SearchEduFilesAsync(string? keyword, int? categoryId, int? courseId, EduFileType? fileType, EduFileOrderingEnum orderBy, bool isDescending)
        {
            return await _eduFileRepository.SearchEduFilesAsync(keyword, categoryId, courseId, fileType, orderBy, isDescending);
        }

        public async Task<string> UpdateMyFileAsync(int id, int userId, string title, string? description, int courseId, EduFileType fileType)
        {
            var file = await _eduFileRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UploadedByUserId == userId);
            if (file == null) return "NotFound";

            file.Title = title;
            file.Description = description;
            file.CourseId = courseId;
            file.FileType = fileType;

            await _eduFileRepository.UpdateAsync(file);
            return "Success";
        }
        public async Task<string> PublishEduFileAsync(int id, int userId)
        {
            var file = await _eduFileRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UploadedByUserId == userId);
            if (file == null) return "NotFound";
            if (file.Status != EduFileStatus.Approved) return "NotApproved";
            if (file.IsPublished) return "AlreadyPublished";

            file.IsPublished = true;
            await _eduFileRepository.UpdateAsync(file);
            return "Success";
        }
        public async Task<List<EduFile>> GetMyFilesAsync(int userId)
    => await _eduFileRepository.GetMyFilesAsync(userId);

        public async Task<string> UnpublishEduFileAsync(int id, int userId)
        {
            var file = await _eduFileRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UploadedByUserId == userId);
            if (file == null) return "NotFound";
            if (!file.IsPublished) return "AlreadyUnpublished";
            file.IsPublished = false;
            await _eduFileRepository.UpdateAsync(file);
            return "Success";
        }
        public async Task<string> AdminDeleteEduFileAsync(int id)
        {
            var file = await _eduFileRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (file == null) return "NotFound";
            if (file.Status == EduFileStatus.Rejected) return "AlreadyDeleted";

            file.Status = EduFileStatus.Rejected;
            file.IsPublished = false;
            await _eduFileRepository.UpdateAsync(file);
            return "Success";
        }

        public async Task<string> UpdateEduFileAsync(EduFile eduFile)
        {
            await _eduFileRepository.UpdateAsync(eduFile);
            return "Success";
        }
    }
}
