using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Category.Commands.Models
{
    public class DeleteCategoryCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
}
