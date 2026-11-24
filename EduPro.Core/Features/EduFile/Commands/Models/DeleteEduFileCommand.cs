using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class DeleteEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteEduFileCommand(int id)
        {
            Id = id;
        }
    }
}
