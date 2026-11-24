using EduPro.Data.Enums;

namespace EduPro.Data.Results.EduFile
{
    public class GetEduFileListResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public EduFileType FileType { get; set; }
        public EduFileStatus Status { get; set; }
        public int DownloadCount { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedByUserName { get; set; }
        public int UploadedByUserId { get; set; }

        public string CourseName { get; set; }
        public string CategoryName { get; set; }
        public string? RequestedCourseName { get; set; }
        public string? RequestedCategoryName { get; set; }
    }
}