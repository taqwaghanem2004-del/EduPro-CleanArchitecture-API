using EduPro.Core.Bases;
using EduPro.Data.Results.Profile;
using MediatR;

namespace EduPro.Core.Features.Profile.Queries.Models
{
    public class GetMyProfileQuery : IRequest<Response<GetMyProfileResult>>
    {
    }
}
