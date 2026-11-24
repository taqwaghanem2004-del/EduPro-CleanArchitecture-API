using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Category.Commands.Models
{
    public class UpdateCategoryCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
    }
}
