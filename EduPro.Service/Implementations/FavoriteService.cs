using EduPro.Data.Entities;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;

namespace EduPro.Service.Implementations
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IEduFileRepository _eduFileRepository;
        private readonly IFavoriteCollectionRepository _favoriteCollectionRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository,
                               IEduFileRepository eduFileRepository,
                               IFavoriteCollectionRepository favoriteCollectionRepository)
        {
            _favoriteRepository = favoriteRepository;
            _eduFileRepository = eduFileRepository;
            _favoriteCollectionRepository = favoriteCollectionRepository;
        }

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(int userId)
            => await _favoriteRepository.GetFavoritesByUserIdAsync(userId);

        public async Task<string> AddFavoriteAsync(int userId, int eduFileId)
        {
            var eduFile = await _eduFileRepository.GetByIdAsync(eduFileId);
            if (eduFile == null) return "EduFileNotFound";

            var isExist = await _favoriteRepository.IsFavoriteExist(userId, eduFileId);
            if (isExist) return "AlreadyExist";

            await _favoriteRepository.AddAsync(new Favorite { UserId = userId, EduFileId = eduFileId });
            return "Success";
        }

        public async Task<string> DeleteFavoriteAsync(int userId, int eduFileId)
        {
            var favorite = await _favoriteRepository.GetFavoriteAsync(userId, eduFileId);
            if (favorite == null) return "NotFound";
            await _favoriteRepository.DeleteAsync(favorite);
            return "Success";
        }
        public async Task<string> AddCollectionToFavoritesAsync(int collectionId, int userId)
        {
            var isExist = await _favoriteCollectionRepository.IsFavoriteCollectionExist(userId, collectionId);
            if (isExist) return "AlreadyExist";

            await _favoriteCollectionRepository.AddAsync(new FavoriteCollection
            {
                UserId = userId,
                CollectionId = collectionId
            });
            return "Success";
        }

        public async Task<string> RemoveCollectionFromFavoritesAsync(int collectionId, int userId)
        {
            var favorite = await _favoriteCollectionRepository.GetFavoriteCollectionAsync(userId, collectionId);
            if (favorite == null) return "NotFound";
            await _favoriteCollectionRepository.DeleteAsync(favorite);
            return "Success";
        }

        public async Task<List<FavoriteCollection>> GetFavoriteCollectionsByUserIdAsync(int userId)
            => await _favoriteCollectionRepository.GetFavoriteCollectionsByUserIdAsync(userId);
    }
}