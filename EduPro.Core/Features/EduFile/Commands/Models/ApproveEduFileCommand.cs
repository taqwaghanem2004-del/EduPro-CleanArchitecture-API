using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class ApproveEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public ApproveEduFileCommand(int id)
        {
            Id = id;
        }
    }
}
