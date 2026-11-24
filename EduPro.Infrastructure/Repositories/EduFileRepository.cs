using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class EduFileRepository : GenericRepositoryAsync<EduFile>, IEduFileRepository
    {
        private readonly DbSet<EduFile> _eduFiles;

        public EduFileRepository(AppDbContext dbContext) : base(dbContext)
        {
            _eduFiles = dbContext.Set<EduFile>();
        }

        public async Task<List<EduFile>> GetEduFilesByStatusAsync(EduFileStatus status)
        {
            return await _dbContext.EduFiles
                .Include(x => x.Course)
                    .ThenInclude(x => x.Category)
                .Include(x => x.UploadedBy)
               .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task<List<EduFile>> GetEduFilesByCourseIdAsync(int courseId)
        {
            return await _dbContext.EduFiles
                .Include(x => x.Course)
                    .ThenInclude(x => x.Category)
                .Include(x => x.UploadedBy)
                 .Where(x => x.CourseId == courseId && x.IsPublished && !x.IsDeleted)
                  .ToListAsync();
        }
        public async Task<List<EduFile>> SearchEduFilesAsync(string? keyword, int? categoryId, int? courseId, EduFileType? fileType, EduFileOrderingEnum orderBy, bool isDescending)
        {
            var query = _eduFiles
                .Include(x => x.UploadedBy)
                .Include(x => x.Course)
                .ThenInclude(x => x.Category)
                 .Where(x => x.IsPublished && x.Status == EduFileStatus.Approved && !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Title.Contains(keyword) ||
                                          x.Description.Contains(keyword));

            if (categoryId.HasValue)
                query = query.Where(x => x.Course.CategoryId == categoryId);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            if (fileType.HasValue)
                query = query.Where(x => x.FileType == fileType);


            query = orderBy switch
            {
                EduFileOrderingEnum.DownloadCount => isDescending ?
                    query.OrderByDescending(x => x.DownloadCount) : query.OrderBy(x => x.DownloadCount),
                _ => isDescending ?
                    query.OrderByDescending(x => x.UploadedAt) : query.OrderBy(x => x.UploadedAt)
            };

            return await query.ToListAsync();
        }
        public async Task<List<EduFile>> GetMyFilesAsync(int userId)
        {
            return await _dbContext.EduFiles
                .Include(x => x.Course)
                    .ThenInclude(x => x.Category)
                      .Where(x => x.UploadedByUserId == userId && !x.IsDeleted)
                      .OrderByDescending(x => x.UploadedAt)
                .ToListAsync();
        }

    }
}
