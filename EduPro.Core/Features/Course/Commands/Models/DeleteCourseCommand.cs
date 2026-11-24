using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Course.Commands.Models
{
    public class DeleteCourseCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DeleteCourseCommand(int id)
        {
            Id = id;
        }
    }
}