using EduPro.Data.Enums;

namespace EduPro.Data.Results.Collection
{
    public class GetCollectionByIdResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
        public CollectionType Type { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string UploaderName { get; set; }
        public int UploaderId { get; set; }
        public List<CollectionFileResult> Files { get; set; }
    }

    public class CollectionFileResult
    {
        public int EduFileId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public int FileType { get; set; }
        public int Order { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
        public int DownloadCount { get; set; }
    }
}
