using EduPro.Core.Bases;
using EduPro.Data.Enums;
using EduPro.Data.Results.EduFile;
using MediatR;

namespace EduPro.Core.Features.EduFile.Queries.Models
{
    public class SearchEduFileQuery : IRequest<Response<SearchResult>>
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int? CourseId { get; set; }
        public EduFileType? FileType { get; set; }
        public EduFileOrderingEnum OrderBy { get; set; } = EduFileOrderingEnum.UploadedAt;
        public bool IsDescending { get; set; } = true;
    }
}
