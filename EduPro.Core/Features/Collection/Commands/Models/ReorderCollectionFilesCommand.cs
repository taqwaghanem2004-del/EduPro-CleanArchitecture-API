using EduPro.Core.Bases;
using EduPro.Service.Abstracts;
using MediatR;

namespace EduPro.Core.Features.Collection.Commands.Models
{
    public class ReorderCollectionFilesCommand : IRequest<Response<string>>
    {
        public int CollectionId { get; set; }
        public List<ReorderFileRequest> Files { get; set; }
    }
}
