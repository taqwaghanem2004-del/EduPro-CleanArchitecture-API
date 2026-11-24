using EduPro.Core.Bases;
using EduPro.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduPro.Core.Features.EduFile.Commands.Models
{
    public class UploadEduFileCommand : IRequest<Response<string>>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public EduFileType FileType { get; set; }
        public int? CourseId { get; set; }
        public string? RequestedCourseName { get; set; }
        public IFormFile File { get; set; }
        public string? RequestedCategoryName { get; set; }
    }
}