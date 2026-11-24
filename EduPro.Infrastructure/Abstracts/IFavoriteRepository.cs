using EduPro.Data.Entities;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Abstracts
{
    public interface IFavoriteRepository : IGenericRepositoryAsync<Favorite>
    {
        Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId);
        Task<Favorite?> GetFavoriteAsync(int userId, int eduFileId);
        Task<bool> IsFavoriteExist(int userId, int eduFileId);
    }
}