using EduPro.Data.Results.Course;

namespace EduPro.Core.Mapping.Course
{
    public partial class CourseProfile
    {
        public void GetCourseListMapping()
        {
            CreateMap<EduPro.Data.Entities.Course, GetCourseListResult>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
