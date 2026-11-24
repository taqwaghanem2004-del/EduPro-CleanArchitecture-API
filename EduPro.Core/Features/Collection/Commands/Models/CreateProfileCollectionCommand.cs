using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class CreateProfileCollectionCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CourseId { get; set; }
    }
}
