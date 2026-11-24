using EduPro.Core.Bases;
using EduPro.Data.Enums;
using EduPro.Data.Results.EduFile;
using MediatR;

namespace EduPro.Core.Features.EduFile.Queries.Models
{
    public class GetEduFileListQuery : IRequest<Response<List<GetEduFileListResult>>>
    {
        public EduFileStatus Status { get; set; }
        public GetEduFileListQuery(EduFileStatus status)
        {
            Status = status;
        }
    }
}
