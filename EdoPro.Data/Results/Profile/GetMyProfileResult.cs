using EduPro.Data.Enums;

namespace EduPro.Data.Results.Profile
{
    public class GetMyProfileResult
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public int TotalFiles { get; set; }
        public int TotalDownloads { get; set; }
        public int CollectionsCount { get; set; }
        public List<MyFileResult> Files { get; set; }
        public List<MyCollectionResult> Collections { get; set; }
    }
    public class MyFileResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public EduFileType FileType { get; set; }
        public EduFileStatus Status { get; set; }
        public bool IsPublished { get; set; }
        public int DownloadCount { get; set; }
        public DateTime UploadedAt { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
    }

    public class MyCollectionResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsPublished { get; set; }
        public int FilesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public CollectionType Type { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
    }
}
