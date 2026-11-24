using EduPro.Core.Bases;
using EduPro.Data.Results.EduFile;
using MediatR;

namespace EduPro.Core.Features.EduFile.Queries.Models
{
    public class GetMyFilesQuery : IRequest<Response<List<GetMyFilesResult>>>
    {
    }
}
