using EduPro.Core.Bases;
using EduPro.Data.Results.Profile;
using MediatR;

namespace EduPro.Core.Features.Profile.Queries.Models
{
    public class GetPublicProfileQuery : IRequest<Response<GetPublicProfileResult>>
    {
        public int UserId { get; set; }
        public GetPublicProfileQuery(int userId) => UserId = userId;
    }
}
