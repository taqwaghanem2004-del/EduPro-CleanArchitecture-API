using EduPro.Data.Enums;

namespace EduPro.Data.Results.Collection
{
    public class GetCollectionListResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FilesCount { get; set; }
        public bool IsPublished { get; set; }
        public string UploaderName { get; set; }
        public int UploaderId { get; set; }

        public CollectionType Type { get; set; }
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }
    }
}
