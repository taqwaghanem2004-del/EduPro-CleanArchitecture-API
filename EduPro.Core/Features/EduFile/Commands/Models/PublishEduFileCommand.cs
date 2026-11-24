using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class PublishEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public PublishEduFileCommand(int id) => Id = id;
    }
}
