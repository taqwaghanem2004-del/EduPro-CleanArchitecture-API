using EduPro.Core.Bases;
using EduPro.Data.Results.Authorization;
using MediatR;

namespace EduPro.Core.Features.Authorization.Queries.Models
{
    public class ManageUserRolesQuery : IRequest<Response<ManageUserRolesResult>>
    {
        public int UserId { get; set; }
        public ManageUserRolesQuery(int userId)
        {
            UserId = userId;
        }
    }
}