using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class AddCollectionToFavoritesCommand : IRequest<Response<string>>
    {
        public int CollectionId { get; set; }

        public AddCollectionToFavoritesCommand(int collectionId)
        {
            CollectionId = collectionId;
        }
    }
}