using AutoMapper;

namespace EduPro.Core.Mapping.Course
{
    public partial class CourseProfile : Profile
    {
        public CourseProfile()
        {
            GetCourseListMapping();
            GetCourseByIdMapping();
            CreateCourseMapping();
            UpdateCourseMapping();
        }
    }
}