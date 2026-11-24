using EduPro.Core.Bases;
using EduPro.Data.Results.EduFile;
using MediatR;

namespace EduPro.Core.Features.EduFile.Queries.Models
{
    public class GetEduFileByCourseIdQuery : IRequest<Response<List<GetEduFileListResult>>>
    {
        public int CourseId { get; set; }
        public GetEduFileByCourseIdQuery(int courseId)
        {
            CourseId = courseId;
        }
    }
}
