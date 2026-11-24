using EduPro.Data.Entities;
using EduPro.Data.Results.Collection;

namespace EduPro.Core.Mapping.Collection
{
    public partial class CollectionProfile
    {
        public void GetCollectionByIdMapping()
        {
            CreateMap<Data.Entities.Collection, GetCollectionByIdResult>()
        .ForMember(dest => dest.UploaderName,
            opt => opt.MapFrom(src => src.User.FullName))

        .ForMember(dest => dest.CourseName,
            opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null))
            .ForMember(dest => dest.Files,
        opt => opt.MapFrom(src => src.CollectionFiles))
            .ForMember(dest => dest.UploaderId,
    opt => opt.MapFrom(src => src.UserId));

            CreateMap<CollectionFile, CollectionFileResult>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.EduFile.Title))
                .ForMember(dest => dest.FilePath,
                    opt => opt.MapFrom(src => src.EduFile.FilePath))
                .ForMember(dest => dest.FileType,
                    opt => opt.MapFrom(src => (int)src.EduFile.FileType))
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.EduFile.Course.Name))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.EduFile.Course.Category.Name))
                .ForMember(dest => dest.DownloadCount,
                    opt => opt.MapFrom(src => src.EduFile.DownloadCount));
        }
    }
}

