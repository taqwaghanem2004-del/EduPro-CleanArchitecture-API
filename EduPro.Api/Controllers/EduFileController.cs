using EduPro.Api.Base;
using EduPro.Core.Features.EduFile.Commands.Models;
using EduPro.Core.Features.EduFile.Queries.Models;
using EduPro.Core.Filters;
using EduPro.Data.AppMetaData;
using EduPro.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    public class EduFileController : AppControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EduFileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        [HttpGet(Router.EduFileRouting.GetByCourseId)]
        public async Task<IActionResult> GetByCourseId([FromRoute] int courseId)
        {
            return NewResult(await Mediator.Send(new GetEduFileByCourseIdQuery(courseId)));
        }

        [AllowAnonymous]
        [HttpGet(Router.EduFileRouting.Search)]
        public async Task<IActionResult> Search([FromQuery] SearchEduFileQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }
        [Authorize]

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageFiles" })]
        [HttpGet(Router.EduFileRouting.GetList)]
        public async Task<IActionResult> GetList([FromRoute] EduFileStatus status)
        {
            return NewResult(await Mediator.Send(new GetEduFileListQuery(status)));
        }
        [Authorize]

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageFiles" })]
        [HttpGet(Router.EduFileRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetEduFileByIdQuery(id)));
        }
        [Authorize]

        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
        [HttpPost(Router.EduFileRouting.Upload)]
        public async Task<IActionResult> Upload([FromForm] UploadEduFileCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [Authorize]

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageFiles" })]
        [HttpPut(Router.EduFileRouting.Approve)]
        public async Task<IActionResult> Approve([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new ApproveEduFileCommand(id)));
        }
        [Authorize]

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageFiles" })]
        [HttpPut(Router.EduFileRouting.Reject)]
        public async Task<IActionResult> Reject([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new RejectEduFileCommand(id)));
        }
        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]

        [HttpDelete(Router.EduFileRouting.StudentDelete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteEduFileCommand(id)));
        }
        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]

        [HttpGet(Router.EduFileRouting.GetMyFiles)]
        public async Task<IActionResult> GetMyFiles()
    => NewResult(await Mediator.Send(new GetMyFilesQuery()));
        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]

        [HttpPut(Router.EduFileRouting.UpdateMyFile)]
        public async Task<IActionResult> UpdateMyFile([FromBody] UpdateMyFileCommand command)
    => NewResult(await Mediator.Send(command));
        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]

        [HttpPut(Router.EduFileRouting.Publish)]
        public async Task<IActionResult> Publish([FromRoute] int id)
            => NewResult(await Mediator.Send(new PublishEduFileCommand(id)));

        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
        [HttpPut(Router.EduFileRouting.Unpublish)]
        public async Task<IActionResult> Unpublish([FromRoute] int id)
    => NewResult(await Mediator.Send(new UnpublishEduFileCommand(id)));
        [Authorize]
        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageFiles" })]
        [HttpDelete(Router.EduFileRouting.AdminDelete)]
        public async Task<IActionResult> AdminDelete([FromRoute] int id)
    => NewResult(await Mediator.Send(new AdminDeleteEduFileCommand(id)));

        [AllowAnonymous]
        [HttpGet(Router.EduFileRouting.Download)]
        public async Task<IActionResult> Download([FromRoute] int id)
        {
            var result = await Mediator.Send(new DownloadEduFileCommand(id));
            if (!result.Succeeded) return NotFound();

            var filePath = Path.Combine(
                _webHostEnvironment.WebRootPath,
                result.Data.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);
            var contentType = GetContentType(fileName);

            return File(fileBytes, contentType, fileName);
        }
        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
        [HttpPost(Router.EduFileRouting.AskQuestion)]
        public async Task<IActionResult> AskQuestion([FromBody] AskQuestionRequest request)
        {
            return NewResult(await Mediator.Send(new AskQuestionCommand
            {
                FileId = request.FileId,
                Question = request.Question
            }));
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLower();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".mp4" => "video/mp4",
                ".mov" => "video/quicktime",
                ".avi" => "video/x-msvideo",
                ".mkv" => "video/x-matroska",
                _ => "application/octet-stream"
            };
        }
  

    }
}