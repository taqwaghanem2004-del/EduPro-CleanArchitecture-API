using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Category.Commands.Models
{
    public class CreateCategoryCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; } 
    }
}
