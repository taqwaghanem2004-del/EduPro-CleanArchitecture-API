using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Course.Commands.Models
{
    public class UpdateCourseCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}