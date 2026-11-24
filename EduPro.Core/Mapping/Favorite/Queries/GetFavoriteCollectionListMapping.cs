using EduPro.Data.Entities;
using EduPro.Data.Results.Favorite;

namespace EduPro.Core.Mapping.Favorite
{
    public partial class FavoriteProfile
    {
        public void GetFavoriteCollectionListMapping()
        {
            CreateMap<FavoriteCollection, GetFavoriteCollectionListResult>()
                .ForMember(d => d.CollectionId, o => o.MapFrom(s => s.CollectionId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Collection.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Collection.Description))
                .ForMember(d => d.FilesCount, o => o.MapFrom(s => s.Collection.CollectionFiles.Count))
                .ForMember(d => d.UploaderName, o => o.MapFrom(s => s.Collection.User.FullName))
                .ForMember(d => d.AddedAt, o => o.MapFrom(s => s.AddedAt))
            .ForMember(d => d.UploaderId,
                 o => o.MapFrom(s => s.Collection.UserId));

        }
    }
}
