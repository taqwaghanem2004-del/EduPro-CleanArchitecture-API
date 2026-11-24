using EduPro.Core.Bases;
using EduPro.Core.Features.Category.Queries.Results;
using MediatR;

namespace EduPro.Core.Features.Category.Queries.Models
{
    public class GetCategoryListQuery : IRequest<Response<List<GetCategoryListResponse>>>
    {
    }
}
