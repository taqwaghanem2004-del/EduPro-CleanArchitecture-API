using EduPro.Core.Bases;
using EduPro.Data.Results.Authorization;
using MediatR;

namespace EduPro.Core.Features.Authorization.Quaries.Models
{
    public class GetUsersListQuery : IRequest<Response<List<GetUsersListResult>>>
    {
        public string? Keyword { get; set; }
        public GetUsersListQuery(string? keyword)
        {
            Keyword = keyword;
        }
    }
}
