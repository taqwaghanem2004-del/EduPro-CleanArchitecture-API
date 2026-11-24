using EduPro.Data.Results.Course;

namespace EduPro.Core.Mapping.Course
{
    public partial class CourseProfile
    {
        public void GetCourseByIdMapping()
        {
            CreateMap<EduPro.Data.Entities.Course, GetCourseByIdResult>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}