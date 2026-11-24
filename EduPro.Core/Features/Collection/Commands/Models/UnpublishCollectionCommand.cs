using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class UnpublishCollectionCommand : IRequest<Response<string>>
    {
        public int CollectionId { get; set; }
        public UnpublishCollectionCommand(int collectionId)
        {
            CollectionId = collectionId;
        }
    }
}
