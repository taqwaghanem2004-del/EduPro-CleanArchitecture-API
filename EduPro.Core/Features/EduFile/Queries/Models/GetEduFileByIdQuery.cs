using EduPro.Core.Bases;
using EduPro.Data.Results.EduFile;
using MediatR;

namespace EduPro.Core.Features.EduFile.Queries.Models
{
    public class GetEduFileByIdQuery : IRequest<Response<GetEduFileByIdResult>>
    {
        public int Id { get; set; }
        public GetEduFileByIdQuery(int id)
        {
            Id = id;
        }
    }
}
