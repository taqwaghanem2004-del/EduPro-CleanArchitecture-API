using EduPro.Data.Entities;

namespace EduPro.Service.Abstracts
{
    public interface IFavoriteService
    {
        Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId);
        Task<string> AddFavoriteAsync(int userId, int eduFileId);
        Task<string> DeleteFavoriteAsync(int userId, int eduFileId);
        // Collection Favorites
        Task<string> AddCollectionToFavoritesAsync(int collectionId, int userId);
        Task<string> RemoveCollectionFromFavoritesAsync(int collectionId, int userId);
        Task<List<FavoriteCollection>> GetFavoriteCollectionsByUserIdAsync(int userId);

    }
}