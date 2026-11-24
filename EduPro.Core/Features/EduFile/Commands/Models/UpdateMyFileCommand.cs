using EduPro.Core.Bases;
using EduPro.Data.Enums;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class UpdateMyFileCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CourseId { get; set; }
        public EduFileType FileType { get; set; }
    }
}
