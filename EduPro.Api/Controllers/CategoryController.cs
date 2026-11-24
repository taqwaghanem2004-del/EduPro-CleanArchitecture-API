using EduPro.Api.Base;
using EduPro.Core.Features.Category.Commands.Models;
using EduPro.Core.Features.Category.Queries.Models;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EduPro.Api.Controllers
{
    [ApiController]
    [Authorize]

    public class CategoryController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.GetList)]
        public async Task<IActionResult> GetList()
        {
            return NewResult(await Mediator.Send(new GetCategoryListQuery()));
        }

        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
        }


        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageCategories" })]

        [HttpPost(Router.CategoryRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            command.UserId = int.Parse(User.FindFirst("id")!.Value);
            return NewResult(await Mediator.Send(command));
        }

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageCategories" })]

        [HttpPut(Router.CategoryRouting.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
        {
            command.UserId = int.Parse(User.FindFirst("id")!.Value);
            return NewResult(await Mediator.Send(command));
        }

        [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "ManageCategories" })]
        [HttpDelete(Router.CategoryRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)

        {
            var command = new DeleteCategoryCommand(id);
            command.UserId = int.Parse(User.FindFirst("id")!.Value);
            return NewResult(await Mediator.Send(new DeleteCategoryCommand(id)));
        }
    }
}