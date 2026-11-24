using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class PublishCollectionCommand : IRequest<Response<string>>
    {
        public int CollectionId { get; set; }
        public PublishCollectionCommand(int collectionId)
        {
            CollectionId = collectionId;
        }
    }
}
