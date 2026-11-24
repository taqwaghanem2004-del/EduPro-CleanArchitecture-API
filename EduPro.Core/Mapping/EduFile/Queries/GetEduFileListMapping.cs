using EduPro.Data.Results.EduFile;

namespace EduPro.Core.Mapping.EduFile
{
    public partial class EduFileProfile
    {
        public void GetEduFileListMapping()
        {
            CreateMap<Data.Entities.EduFile, GetEduFileListResult>()
                .ForMember(dest => dest.UploadedByUserName,
                    opt => opt.MapFrom(src => src.UploadedBy.UserName))
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src =>
                        src.Course != null ? src.Course.Name : src.RequestedCourseName))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src =>
                        src.Course != null ? src.Course.Category.Name : src.RequestedCategoryName ?? "غير محدد"))
                .ForMember(dest => dest.RequestedCourseName,
                    opt => opt.MapFrom(src => src.RequestedCourseName))
                .ForMember(dest => dest.RequestedCategoryName,
                    opt => opt.MapFrom(src => src.RequestedCategoryName))
                .ForMember(dest => dest.UploadedByUserId,
                 opt => opt.MapFrom(src => src.UploadedByUserId));

        }
    }
}