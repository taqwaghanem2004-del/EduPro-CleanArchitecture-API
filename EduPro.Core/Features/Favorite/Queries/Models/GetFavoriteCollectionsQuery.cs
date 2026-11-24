using EduPro.Core.Bases;
using EduPro.Data.Results.Favorite;
using MediatR;

namespace EduPro.Core.Features.Favorite.Queries.Models
{
    public class GetFavoriteCollectionsQuery : IRequest<Response<List<GetFavoriteCollectionListResult>>>
    {
    }
}
