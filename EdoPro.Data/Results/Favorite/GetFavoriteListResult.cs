using EduPro.Data.Enums;

namespace EduPro.Data.Results.Favorite
{
    public class GetFavoriteListResult
    {
        public int EduFileId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public EduFileType FileType { get; set; }
        public int DownloadCount { get; set; }
        public DateTime UploadedAt { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
        public DateTime AddedAt { get; set; }
        public int UploadedByUserId { get; set; }
        public string UploadedByUserName { get; set; }

    }
}