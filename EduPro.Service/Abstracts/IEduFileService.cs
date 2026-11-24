using EduPro.Data.Entities;
using EduPro.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace EduPro.Service.Abstracts
{
    public interface IEduFileService
    {
        Task<List<EduFile>> GetEduFilesByStatusAsync(EduFileStatus status);
        Task<List<EduFile>> GetEduFilesByCourseIdAsync(int courseId);
        Task<EduFile> GetEduFileByIdAsync(int id);
        Task<string> UploadEduFileAsync(EduFile eduFile, IFormFile file);
        Task<string> ApproveEduFileAsync(int id);
        Task<string> RejectEduFileAsync(int id);
        Task<string> DeleteEduFileAsync(int id, int userId);
        Task<bool> IsEduFileExist(int id);
        Task<string> DownloadEduFileAsync(int id);
        Task<List<EduFile>> SearchEduFilesAsync(string? keyword, int? categoryId, int? courseId, EduFileType? fileType, EduFileOrderingEnum orderBy, bool isDescending);
        Task<string> UpdateEduFileAsync(EduFile eduFile);
        Task<List<EduFile>> GetMyFilesAsync(int userId);
        Task<string> UpdateMyFileAsync(int id, int userId, string title, string? description, int courseId, EduFileType fileType);

        Task<string> PublishEduFileAsync(int id, int userId);
        Task<string> UnpublishEduFileAsync(int id, int userId);
        Task<string> AdminDeleteEduFileAsync(int id);

    }
}
