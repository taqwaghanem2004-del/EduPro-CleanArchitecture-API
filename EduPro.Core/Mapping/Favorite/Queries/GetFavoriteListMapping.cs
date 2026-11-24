using EduPro.Data.Results.Favorite;

namespace EduPro.Core.Mapping.Favorite
{
    public partial class FavoriteProfile
    {
        public void GetFavoriteListMapping()
        {
            CreateMap<Data.Entities.Favorite, GetFavoriteListResult>()
                .ForMember(d => d.EduFileId, o => o.MapFrom(s => s.EduFileId))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.EduFile.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.EduFile.Description))
                .ForMember(d => d.FilePath, o => o.MapFrom(s => s.EduFile.FilePath))
                .ForMember(d => d.FileType, o => o.MapFrom(s => s.EduFile.FileType))
                .ForMember(d => d.DownloadCount, o => o.MapFrom(s => s.EduFile.DownloadCount))
                .ForMember(d => d.UploadedAt, o => o.MapFrom(s => s.EduFile.UploadedAt))
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.EduFile.Course.Name))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.EduFile.Course.Category.Name))
                .ForMember(d => d.AddedAt, o => o.MapFrom(s => s.AddedAt))
                .ForMember(d => d.UploadedByUserId,
                  o => o.MapFrom(s => s.EduFile.UploadedByUserId))
               .ForMember(d => d.UploadedByUserName,
                o => o.MapFrom(s => s.EduFile.UploadedBy.UserName));

        }
    }
}