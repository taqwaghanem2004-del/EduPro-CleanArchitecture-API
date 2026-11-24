using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Abstracts
{
    public interface IEduFileRepository : IGenericRepositoryAsync<EduFile>
    {
        Task<List<EduFile>> GetEduFilesByStatusAsync(EduFileStatus status);
        Task<List<EduFile>> GetEduFilesByCourseIdAsync(int courseId);
        Task<List<EduFile>> SearchEduFilesAsync(string? keyword, int? categoryId, int? courseId, EduFileType? fileType, EduFileOrderingEnum orderBy, bool isDescending);
        Task<List<EduFile>> GetMyFilesAsync(int userId);

    }
}
