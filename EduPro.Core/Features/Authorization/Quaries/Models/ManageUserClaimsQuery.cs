using EduPro.Core.Bases;
using EduPro.Data.Results.Authorization;
using MediatR;

namespace EduPro.Core.Features.Authorization.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManageUserClaimsResult>>
    {
        public int UserId { get; set; }
        public ManageUserClaimsQuery(int userId)
        {
            UserId = userId;
        }
    }
}