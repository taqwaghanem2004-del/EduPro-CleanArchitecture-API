using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class FavoriteCollectionRepository : GenericRepositoryAsync<FavoriteCollection>,
                                                 IFavoriteCollectionRepository
    {
        private readonly AppDbContext _dbContext;

        public FavoriteCollectionRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FavoriteCollection?> GetFavoriteCollectionAsync(int userId, int collectionId)
        {
            return await _dbContext.FavoriteCollections
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CollectionId == collectionId);
        }

        public async Task<List<FavoriteCollection>> GetFavoriteCollectionsByUserIdAsync(int userId)
        {
            return await _dbContext.FavoriteCollections
                .Include(x => x.Collection)
                    .ThenInclude(x => x.CollectionFiles)
                        .ThenInclude(x => x.EduFile)
                            .ThenInclude(x => x.Course)
                                .ThenInclude(x => x.Category)
                .Include(x => x.Collection)
                    .ThenInclude(x => x.User)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync();
        }

        public async Task<bool> IsFavoriteCollectionExist(int userId, int collectionId)
        {
            return await _dbContext.FavoriteCollections
                .AnyAsync(x => x.UserId == userId && x.CollectionId == collectionId);
        }
    }
}
