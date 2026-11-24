using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Service.Implementations
{
    public class CollectionService : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly ICollectionFileRepository _collectionFileRepository;
        private readonly IEduFileRepository _eduFileRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IFavoriteCollectionRepository _favoriteCollectionRepository;

        public CollectionService(ICollectionRepository collectionRepository,
                                  ICollectionFileRepository collectionFileRepository,
                                  IEduFileRepository eduFileRepository,
                                  IFavoriteRepository favoriteRepository,
                                  IFavoriteCollectionRepository favoriteCollectionRepository)
        {
            _collectionRepository = collectionRepository;
            _collectionFileRepository = collectionFileRepository;
            _eduFileRepository = eduFileRepository;
            _favoriteRepository = favoriteRepository;
            _favoriteCollectionRepository = favoriteCollectionRepository;
        }



        public async Task<Collection?> GetCollectionByIdAsync(int id, int userId)
        {
            return await _collectionRepository.GetCollectionByIdAsync(id, userId);
        }



        public async Task<string> UpdateCollectionAsync(int id, int userId, string name, string? description)
        {
            var collection = await _collectionRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (collection == null) return "NotFound";
            collection.Name = name;
            collection.Description = description;
            await _collectionRepository.UpdateAsync(collection);
            return "Success";
        }

        public async Task<string> DeleteCollectionAsync(int id, int userId)
        {
            var collection = await _collectionRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (collection == null) return "NotFound";
            var favoriteCollections = await _favoriteCollectionRepository
             .GetTableAsTracking()
             .Where(x => x.CollectionId == id)
              .ToListAsync();

            foreach (var fav in favoriteCollections)
                await _favoriteCollectionRepository.DeleteAsync(fav);
            await _collectionRepository.DeleteAsync(collection);
            return "Success";
        }



        public async Task<string> AddFileToCollectionAsync(int collectionId, int eduFileId, int userId)
        {
            var collection = await _collectionRepository.GetCollectionByIdAsync(collectionId, userId);
            if (collection == null) return "NotFound";
            if (collection.Type == CollectionType.Profile)
            {
                var newFile = await _eduFileRepository.GetByIdAsync(eduFileId);
                if (newFile == null) return "FileNotFound";
                if (newFile.UploadedByUserId != userId) return "NotYourFile";
                if (newFile.CourseId != collection.CourseId) return "WrongCourse";
            }
            else
            {
                var isFav = await _favoriteRepository.IsFavoriteExist(userId, eduFileId);
                if (!isFav) return "NotInFavorites";
            }
            var existing = await _collectionFileRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(x => x.CollectionId == collectionId && x.EduFileId == eduFileId);
            if (existing != null) return "AlreadyExist";

            var maxOrder = await _collectionFileRepository.GetTableNoTracking()
                .Where(x => x.CollectionId == collectionId)
                .MaxAsync(x => (int?)x.Order) ?? 0;

            await _collectionFileRepository.AddAsync(new CollectionFile
            {
                CollectionId = collectionId,
                EduFileId = eduFileId,
                Order = maxOrder + 1
            });
            return "Success";
        }

        public async Task<string> RemoveFileFromCollectionAsync(int collectionId, int eduFileId, int userId)
        {
            var collection = await _collectionRepository.GetCollectionByIdAsync(collectionId, userId);
            if (collection == null) return "NotFound";

            var file = await _collectionFileRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(x => x.CollectionId == collectionId && x.EduFileId == eduFileId);
            if (file == null) return "NotFound";

            await _collectionFileRepository.DeleteAsync(file);
            return "Success";
        }

        public async Task<string> ReorderFilesAsync(int collectionId, int userId, List<ReorderFileRequest> files)
        {
            var collection = await _collectionRepository.GetCollectionByIdAsync(collectionId, userId);
            if (collection == null) return "NotFound";

            foreach (var file in files)
            {
                var cf = await _collectionFileRepository.GetTableAsTracking()
                    .FirstOrDefaultAsync(x => x.CollectionId == collectionId && x.EduFileId == file.EduFileId);
                if (cf != null)
                {
                    cf.Order = file.Order;
                    await _collectionFileRepository.UpdateAsync(cf);
                }
            }
            return "Success";
        }
        public async Task<string> PublishCollectionAsync(int collectionId, int userId)
        {
            var collection = await _collectionRepository.GetCollectionByIdAsync(collectionId, userId);
            if (collection == null) return "NotFound";
            if (collection.Type == CollectionType.Library) return "CannotPublishLibrary";
            if (!collection.CollectionFiles.Any()) return "EmptyCollection";
            if (collection.CourseId == null) return "NoCourse";
            collection.IsPublished = true;
            await _collectionRepository.UpdateAsync(collection);
            return "Success";
        }

        public async Task<List<Collection>> GetPublicCollectionsAsync()
        {
            return await _collectionRepository.GetTableNoTracking()
                .Include(x => x.CollectionFiles)
                    .ThenInclude(x => x.EduFile)
                        .ThenInclude(x => x.Course)
                            .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Course)
                 .Where(x => x.IsPublished && x.Type == CollectionType.Profile)
                 .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<string> CopyCollectionAsync(int collectionId, int userId)
        {
            var original = await _collectionRepository.GetPublishedCollectionByIdAsync(collectionId);
            if (original == null) return "NotFound";
            if (original.UserId == userId) return "CannotCopyOwn";

            var copy = new Collection
            {
                UserId = userId,
                Name = original.Name + " (Copy)",
                Description = original.Description,
                Type = CollectionType.Library,
                CourseId = null,
                IsPublished = false,
                CreatedAt = DateTime.UtcNow,
                CollectionFiles = original.CollectionFiles
                    .OrderBy(cf => cf.Order)
                    .Select((cf, index) => new CollectionFile
                    {
                        EduFileId = cf.EduFileId,
                        Order = index + 1
                    }).ToList()
            };

            await _collectionRepository.AddAsync(copy);
            return "Success";
        }
        public async Task<List<Collection>> GetProfileCollectionsByUserIdAsync(int userId)
        {
            return await _collectionRepository.GetTableNoTracking()
                .Include(x => x.CollectionFiles).ThenInclude(x => x.EduFile)
                .Include(x => x.Course)
                .Where(x => x.UserId == userId && x.Type == CollectionType.Profile)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<Collection>> GetLibraryCollectionsByUserIdAsync(int userId)
        {
            return await _collectionRepository.GetTableNoTracking()
                .Include(x => x.CollectionFiles).ThenInclude(x => x.EduFile)
                .Where(x => x.UserId == userId && x.Type == CollectionType.Library)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<string> CreateProfileCollectionAsync(int userId, string name, string? description, int courseId)
        {
            var collection = new Collection
            {
                UserId = userId,
                Name = name,
                Description = description,
                Type = CollectionType.Profile,
                CourseId = courseId
            };
            await _collectionRepository.AddAsync(collection);
            return "Success";
        }
        public async Task<string> CreateLibraryCollectionAsync(int userId, string name, string? description)
        {
            var collection = new Collection
            {
                UserId = userId,
                Name = name,
                Description = description,
                Type = CollectionType.Library,
                CourseId = null
            };
            await _collectionRepository.AddAsync(collection);
            return "Success";
        }
        public async Task<List<Collection>> GetCollectionsByCourseIdAsync(int courseId)
        {
            return await _collectionRepository.GetTableNoTracking()
                .Include(x => x.CollectionFiles)
                    .ThenInclude(x => x.EduFile)
                        .ThenInclude(x => x.Course)
                            .ThenInclude(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Course)
                .Where(x => x.IsPublished && x.Type == CollectionType.Profile && x.CourseId == courseId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<string> UnpublishCollectionAsync(int collectionId, int userId)
        {
            var collection = await _collectionRepository.GetCollectionByIdAsync(collectionId, userId);
            if (collection == null) return "NotFound";
            if (!collection.IsPublished) return "AlreadyUnpublished";
            collection.IsPublished = false;
            await _collectionRepository.UpdateAsync(collection);
            return "Success";
        }
    }
}