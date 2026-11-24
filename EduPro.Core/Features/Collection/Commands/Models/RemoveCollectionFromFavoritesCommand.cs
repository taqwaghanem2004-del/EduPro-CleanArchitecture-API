using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class RemoveCollectionFromFavoritesCommand : IRequest<Response<string>>
    {
        public int CollectionId { get; set; }
        public RemoveCollectionFromFavoritesCommand(int collectionId)
        {
            CollectionId = collectionId;
        }
    }
}
