using EduPro.Core.Bases;
using EduPro.Data.Results.Course;
using MediatR;

namespace EduPro.Core.Features.Course.Queries.Models
{
    public class GetCourseByIdQuery : IRequest<Response<GetCourseByIdResult>>
    {
        public int Id { get; set; }
        public GetCourseByIdQuery(int id)
        {
            Id = id;
        }
    }
}