using EduPro.Data.Entities;

namespace EduPro.Service.Abstracts
{
    public interface ICollectionService
    {
        Task<Collection?> GetCollectionByIdAsync(int id, int userId);
        Task<string> UpdateCollectionAsync(int id, int userId, string name, string? description);
        Task<string> DeleteCollectionAsync(int id, int userId);
        Task<string> AddFileToCollectionAsync(int collectionId, int eduFileId, int userId);
        Task<string> RemoveFileFromCollectionAsync(int collectionId, int eduFileId, int userId);
        Task<string> ReorderFilesAsync(int collectionId, int userId, List<ReorderFileRequest> files);
        Task<string> PublishCollectionAsync(int collectionId, int userId);
        Task<List<Collection>> GetPublicCollectionsAsync();
        Task<string> CopyCollectionAsync(int collectionId, int userId);
        Task<List<Collection>> GetProfileCollectionsByUserIdAsync(int userId);
        Task<List<Collection>> GetLibraryCollectionsByUserIdAsync(int userId);
        Task<string> CreateProfileCollectionAsync(int userId, string name, string? description, int courseId);
        Task<string> CreateLibraryCollectionAsync(int userId, string name, string? description);
        Task<List<Collection>> GetCollectionsByCourseIdAsync(int courseId);
        Task<string> UnpublishCollectionAsync(int collectionId, int userId);

    }
    public class ReorderFileRequest
    {
        public int EduFileId { get; set; }
        public int Order { get; set; }
    }
}
