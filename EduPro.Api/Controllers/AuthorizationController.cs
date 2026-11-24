using EduPro.Api.Base;
using EduPro.Core.Features.Authorization.Commands.Models;
using EduPro.Core.Features.Authorization.Quaries.Models;
using EduPro.Core.Features.Authorization.Queries.Models;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    [Authorize]

    [TypeFilter(typeof(AdminFilter), Arguments = new object[] { "SuperAdmin" })]
    public class AuthorizationController : AppControllerBase
    {
        #region Roles
        [HttpGet(Router.AuthorizationRouting.GetRolesList)]
        public async Task<IActionResult> GetRolesList()
        {
            return NewResult(await Mediator.Send(new GetRolesListQuery()));
        }
        [HttpGet(Router.AuthorizationRouting.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetRoleByIdQuery(id)));
        }
        [HttpPost(Router.AuthorizationRouting.AddRole)]
        public async Task<IActionResult> AddRole([FromBody] AddRoleCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpPut(Router.AuthorizationRouting.EditRole)]
        public async Task<IActionResult> EditRole([FromBody] EditRoleCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteRoleCommand(id)));
        }
        #endregion

        #region Claims
        [HttpGet(Router.AuthorizationRouting.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] int userId)
        {
            return NewResult(await Mediator.Send(new ManageUserRolesQuery(userId)));
        }
        [HttpPut(Router.AuthorizationRouting.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpGet(Router.AuthorizationRouting.ManageUserClaims)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] int userId)
        {
            return NewResult(await Mediator.Send(new ManageUserClaimsQuery(userId)));
        }
        [HttpPut(Router.AuthorizationRouting.UpdateUserClaims)]
        public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpGet(Router.AuthorizationRouting.GetUsersList)]
        public async Task<IActionResult> GetUsersList([FromQuery] string? keyword)
        {
            return NewResult(await Mediator.Send(new GetUsersListQuery(keyword)));
        }
        [HttpPut(Router.AuthorizationRouting.BanUser)]
        public async Task<IActionResult> BanUser([FromRoute] int userId)
        {
            var command = new BanUserCommand
            {
                UserId = userId,
                AdminUserId = int.Parse(User.FindFirst("id")!.Value)
            };
            return NewResult(await Mediator.Send(command));
        }
        [HttpPut(Router.AuthorizationRouting.UnbanUser)]
        public async Task<IActionResult> UnbanUser([FromRoute] int userId)
        {
            var command = new UnbanUserCommand
            {
                UserId = userId,
                AdminUserId = int.Parse(User.FindFirst("id")!.Value)
            };
            return NewResult(await Mediator.Send(command));
        }
        #endregion
    }
}