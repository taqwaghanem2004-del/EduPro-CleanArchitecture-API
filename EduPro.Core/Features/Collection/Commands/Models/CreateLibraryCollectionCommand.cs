using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class CreateLibraryCollectionCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
