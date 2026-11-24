using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class AskQuestionCommand : IRequest<Response<string>>
    {
        public int FileId { get; set; }
        public string Question { get; set; }
    }

    public class AskQuestionRequest
    {
        public int FileId { get; set; }
        public string Question { get; set; }
    }
}