using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class CollectionRepository : GenericRepositoryAsync<Collection>, ICollectionRepository
    {
        private readonly AppDbContext _dbContext;

        public CollectionRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<Collection?> GetCollectionByIdAsync(int id, int userId)
        {
            return await _dbContext.Collections
                .Include(x => x.CollectionFiles)
                    .ThenInclude(x => x.EduFile)
                        .ThenInclude(x => x.Course)
                            .ThenInclude(x => x.Category)
                            .Include(x => x.User)
                            .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == id && (x.UserId == userId || x.IsPublished));
        }

        public async Task<CollectionFile?> GetCollectionFileAsync(int collectionId, int eduFileId)
        {
            return await _dbContext.CollectionFiles
                .FirstOrDefaultAsync(x => x.CollectionId == collectionId && x.EduFileId == eduFileId);
        }

        public async Task<int> GetMaxOrderAsync(int collectionId)
        {
            var files = await _dbContext.CollectionFiles
                .Where(x => x.CollectionId == collectionId)
                .ToListAsync();
            return files.Any() ? files.Max(x => x.Order) : 0;
        }
        public async Task<Collection?> GetPublishedCollectionByIdAsync(int id)
        {
            return await _dbContext.Collections
                .Include(x => x.CollectionFiles)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsPublished);
        }
        public async Task<List<Collection>> SearchCollectionsAsync(string? keyword, int? categoryId, int? courseId, EduFileOrderingEnum orderBy, bool isDescending)
        {
            var query = _dbContext.Collections
                .Include(x => x.User)
                .Include(x => x.Course)
                    .ThenInclude(x => x.Category)
                .Include(x => x.CollectionFiles)
                .Where(x => x.IsPublished && x.Type == CollectionType.Profile)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) ||
                                          x.Description.Contains(keyword));

            if (categoryId.HasValue)
                query = query.Where(x => x.Course.CategoryId == categoryId);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId);

            query = orderBy switch
            {
                EduFileOrderingEnum.DownloadCount => isDescending
                    ? query.OrderByDescending(x => x.CollectionFiles.Sum(f => f.EduFile.DownloadCount))
                    : query.OrderBy(x => x.CollectionFiles.Sum(f => f.EduFile.DownloadCount)),
                _ => isDescending
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt)
            };


            return await query.ToListAsync();
        }

    }

}
