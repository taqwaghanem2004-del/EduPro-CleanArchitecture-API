using EduPro.Data.Results.EduFile;

namespace EduPro.Core.Mapping.EduFile
{
    public partial class EduFileProfile
    {
        public void GetEduFileByIdMapping()
        {
            CreateMap<Data.Entities.EduFile, GetEduFileByIdResult>()
                .ForMember(dest => dest.UploadedByUserName,
                    opt => opt.MapFrom(src => src.UploadedBy.UserName))
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Course.Category.Name));
        }
    }
}