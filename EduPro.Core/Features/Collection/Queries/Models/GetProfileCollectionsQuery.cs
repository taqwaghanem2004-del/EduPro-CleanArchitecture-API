using EduPro.Core.Bases;
using EduPro.Data.Results.Collection;
using MediatR;

namespace EduPro.Core.Features.Collection.Queries.Models
{
    public class GetProfileCollectionsQuery : IRequest<Response<List<GetCollectionListResult>>>
    {
    }
}
