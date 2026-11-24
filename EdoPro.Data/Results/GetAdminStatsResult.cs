namespace EduPro.Data.Results
{
    public class GetAdminStatsResult
    {
        public int PendingFilesCount { get; set; }
        public int ApprovedFilesCount { get; set; }
        public int RejectedFilesCount { get; set; }
        public int TotalDownloads { get; set; }
        public string? MostDownloadedFile { get; set; }
        public string? MostActiveCourse { get; set; }
    }
}
