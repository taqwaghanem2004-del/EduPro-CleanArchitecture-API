using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class UnpublishEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public UnpublishEduFileCommand(int id) => Id = id;
    }
}
