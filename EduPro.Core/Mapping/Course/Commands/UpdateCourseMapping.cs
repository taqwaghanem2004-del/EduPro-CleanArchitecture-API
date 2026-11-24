using EduPro.Core.Features.Course.Commands.Models;

namespace EduPro.Core.Mapping.Course
{
    public partial class CourseProfile
    {
        public void UpdateCourseMapping()
        {
            CreateMap<UpdateCourseCommand, EduPro.Data.Entities.Course>();
        }
    }
}
