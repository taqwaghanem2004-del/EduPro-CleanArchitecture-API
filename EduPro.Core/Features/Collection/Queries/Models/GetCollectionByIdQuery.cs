using EduPro.Core.Bases;
using EduPro.Data.Results.Collection;
using MediatR;

namespace EduPro.Core.Features.Collection.Queries.Models
{
    public class GetCollectionByIdQuery : IRequest<Response<GetCollectionByIdResult>>
    {
        public int Id { get; set; }
        public GetCollectionByIdQuery(int id) { Id = id; }
    }
}
