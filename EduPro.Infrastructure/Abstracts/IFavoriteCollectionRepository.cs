using EduPro.Data.Entities;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Abstracts
{
    public interface IFavoriteCollectionRepository : IGenericRepositoryAsync<FavoriteCollection>
    {
        Task<FavoriteCollection?> GetFavoriteCollectionAsync(int userId, int collectionId);
        Task<List<FavoriteCollection>> GetFavoriteCollectionsByUserIdAsync(int userId);
        Task<bool> IsFavoriteCollectionExist(int userId, int collectionId);
    }
}
