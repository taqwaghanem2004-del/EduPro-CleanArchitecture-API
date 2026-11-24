using EduPro.Api.Base;
using EduPro.Core.Features.Course.Commands.Models;
using EduPro.Core.Features.Course.Queries.Models;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    [Authorize]

    public class CourseController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.CourseRouting.GetList)]
        public async Task<IActionResult> GetList([FromRoute] int categoryId)
        {
            return NewResult(await Mediator.Send(new GetCourseListQuery(categoryId)));
        }

        [AllowAnonymous]
        [HttpGet(Router.CourseRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetCourseByIdQuery(id)));
        }

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageCourses" })]
        [HttpPost(Router.CourseRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCourseCommand command)
        {
            command.UserId = int.Parse(User.FindFirst("id")!.Value);
            return NewResult(await Mediator.Send(command));
        }

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageCourses" })]
        [HttpPut(Router.CourseRouting.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCourseCommand command)
        {
            command.UserId = int.Parse(User.FindFirst("id")!.Value);
            return NewResult(await Mediator.Send(command));
        }

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageCourses" })]
        [HttpDelete(Router.CourseRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteCourseCommand(id);
            command.UserId = int.Parse(User.FindFirst("id")!.Value);
            return NewResult(await Mediator.Send(new DeleteCourseCommand(id)));
        }
    }
}