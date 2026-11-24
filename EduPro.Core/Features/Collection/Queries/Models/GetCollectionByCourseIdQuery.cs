using EduPro.Core.Bases;
using EduPro.Data.Results.Collection;
using MediatR;

namespace EduPro.Core.Features.Collection.Queries.Models
{
    public class GetCollectionByCourseIdQuery : IRequest<Response<List<GetCollectionListResult>>>
    {
        public int CourseId { get; set; }
        public GetCollectionByCourseIdQuery(int courseId) => CourseId = courseId;
    }
}
