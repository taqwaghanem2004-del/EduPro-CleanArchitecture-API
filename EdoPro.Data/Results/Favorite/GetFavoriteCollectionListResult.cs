namespace EduPro.Data.Results.Favorite
{
    public class GetFavoriteCollectionListResult
    {
        public int CollectionId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int FilesCount { get; set; }
        public string UploaderName { get; set; }
        public int UploaderId { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
