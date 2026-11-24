using EduPro.Core.Features.Course.Commands.Models;

namespace EduPro.Core.Mapping.Course
{
    public partial class CourseProfile
    {
        public void CreateCourseMapping()
        {
            CreateMap<CreateCourseCommand, EduPro.Data.Entities.Course>();
        }
    }
}