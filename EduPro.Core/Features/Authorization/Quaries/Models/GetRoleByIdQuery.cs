using EduPro.Core.Bases;
using EduPro.Core.Features.Authorization.Quaries.Results;
using EduPro.Data.Results.Authorization;
using MediatR;

namespace EduPro.Core.Features.Authorization.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<GetRoleByIdResult>>
    {
        public int Id { get; set; }
        public GetRoleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
