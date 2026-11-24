using EduPro.Data.Results.Collection;

namespace EduPro.Core.Mapping.Collection
{
    public partial class CollectionProfile
    {
        public void GetCollectionListMapping()
        {
            CreateMap<Data.Entities.Collection, GetCollectionListResult>()
                .ForMember(dest => dest.FilesCount,
                    opt => opt.MapFrom(src => src.CollectionFiles.Count))
                .ForMember(dest => dest.UploaderName,
                    opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null))
            .ForMember(dest => dest.UploaderId, opt => opt.MapFrom(src => src.UserId));

        }
    }
}
