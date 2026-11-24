using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class RemoveFileFromCollectionCommand : IRequest<Response<string>>
    {
        public int CollectionId { get; set; }
        public int EduFileId { get; set; }
    }
}
