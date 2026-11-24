using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Infrastructure.Data;
using EduPro.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Infrastructure.Repositories
{
    public class FavoriteRepository : GenericRepositoryAsync<Favorite>, IFavoriteRepository
    {
        private readonly DbSet<Favorite> _favorites;

        public FavoriteRepository(AppDbContext dbContext) : base(dbContext)
        {
            _favorites = dbContext.Set<Favorite>();
        }

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId)
        {
            return await _favorites
           .Include(x => x.EduFile)
           .ThenInclude(x => x.Course)
            .ThenInclude(x => x.Category)
             .Include(x => x.EduFile)
              .ThenInclude(x => x.UploadedBy)
              .Where(x => x.UserId == userId)
                .ToListAsync();

        }

        public async Task<Favorite?> GetFavoriteAsync(int userId, int eduFileId)
        {
            return await _favorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.EduFileId == eduFileId);
        }

        public async Task<bool> IsFavoriteExist(int userId, int eduFileId)
        {
            return await _favorites
                .AnyAsync(x => x.UserId == userId && x.EduFileId == eduFileId);
        }
    }
}