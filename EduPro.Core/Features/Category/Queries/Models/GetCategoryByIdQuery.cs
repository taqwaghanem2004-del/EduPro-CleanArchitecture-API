using EduPro.Core.Bases;
using EduPro.Core.Features.Category.Queries.Results;
using MediatR;

namespace EduPro.Core.Features.Category.Queries.Models
{
    public class GetCategoryByIdQuery : IRequest<Response<GetCategoryListResponse>>
    {
        public int Id { get; set; }
        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
