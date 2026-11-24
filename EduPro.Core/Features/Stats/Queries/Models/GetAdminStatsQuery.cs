using EduPro.Core.Bases;
using EduPro.Data.Results;
using MediatR;

namespace EduPro.Core.Features.Stats.Queries.Models
{
    public class GetAdminStatsQuery : IRequest<Response<GetAdminStatsResult>>
    {
    }
}
