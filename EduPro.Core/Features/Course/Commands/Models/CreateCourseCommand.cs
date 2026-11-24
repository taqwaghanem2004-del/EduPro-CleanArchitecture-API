using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Course.Commands.Models
{
    public class CreateCourseCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int? EduFileId { get; set; }
        public int UserId { get; set; }

    }
}