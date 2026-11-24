using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class DownloadEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DownloadEduFileCommand(int id)
        {
            Id = id;
        }
    }
}
