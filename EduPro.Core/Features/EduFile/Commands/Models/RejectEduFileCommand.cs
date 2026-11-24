using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class RejectEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public RejectEduFileCommand(int id)
        {
            Id = id;
        }
    }
}
