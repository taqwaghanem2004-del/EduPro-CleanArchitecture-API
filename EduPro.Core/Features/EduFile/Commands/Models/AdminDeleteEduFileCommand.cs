using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class AdminDeleteEduFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public AdminDeleteEduFileCommand(int id) => Id = id;
    }

}
