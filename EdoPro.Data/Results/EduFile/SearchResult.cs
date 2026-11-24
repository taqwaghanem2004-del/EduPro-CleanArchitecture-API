using EduPro.Data.Results.Collection;

namespace EduPro.Data.Results.EduFile
{
    public class SearchResult
    {
        public List<GetCollectionListResult> Collections { get; set; }
        public List<GetEduFileListResult> Files { get; set; }
    }

}
