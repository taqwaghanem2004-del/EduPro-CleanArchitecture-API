namespace EduPro.Data.Results.EduFile
{
    public class GetMyFilesResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }
        public int FileType { get; set; }
        public string Status { get; set; }
        public int DownloadCount { get; set; }
        public DateTime UploadedAt { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
    }
}
