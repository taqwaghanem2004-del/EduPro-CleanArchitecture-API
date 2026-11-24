using EduPro.Data.Entities;
using EduPro.Data.Enums;
using EduPro.Infrastructure.InfrastructureBases;

namespace EduPro.Infrastructure.Abstracts
{
    public interface ICollectionRepository : IGenericRepositoryAsync<Collection>
    {
        Task<Collection?> GetCollectionByIdAsync(int id, int userId);
        Task<CollectionFile?> GetCollectionFileAsync(int collectionId, int eduFileId);
        Task<int> GetMaxOrderAsync(int collectionId);
        Task<Collection?> GetPublishedCollectionByIdAsync(int id);
        Task<List<Collection>> SearchCollectionsAsync(string? keyword, int? categoryId, int? courseId, EduFileOrderingEnum orderBy, bool isDescending);

    }
}
