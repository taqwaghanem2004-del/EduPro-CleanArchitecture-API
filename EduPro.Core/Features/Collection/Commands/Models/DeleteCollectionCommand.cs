using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class DeleteCollectionCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCollectionCommand(int id) { Id = id; }
    }
}
