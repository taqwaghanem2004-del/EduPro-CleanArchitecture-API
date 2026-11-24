namespace EduPro.Data.Results.Profile
{
    public class GetPublicProfileResult
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public int TotalFiles { get; set; }
        public int TotalDownloads { get; set; }
        public int CollectionsCount { get; set; }
        public List<MyFileResult> Files { get; set; }
        public List<MyCollectionResult> Collections { get; set; }
    }

}
