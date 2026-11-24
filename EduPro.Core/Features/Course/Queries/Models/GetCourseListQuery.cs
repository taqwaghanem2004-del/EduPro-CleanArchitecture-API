using EduPro.Core.Bases;
using EduPro.Data.Results.Course;
using MediatR;

namespace EduPro.Core.Features.Course.Queries.Models
{
    public class GetCourseListQuery : IRequest<Response<List<GetCourseListResult>>>
    {
        public int CategoryId { get; set; }
        public GetCourseListQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}