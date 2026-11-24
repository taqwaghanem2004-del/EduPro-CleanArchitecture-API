using EduPro.Api.Base;
using EduPro.Core.Features.Profile.Commands.Models;
using EduPro.Core.Features.Profile.Queries.Models;
using EduPro.Core.Filters;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    public class ProfileController : AppControllerBase
    {
        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
        [HttpGet(Router.ProfileRouting.GetMyProfile)]
        public async Task<IActionResult> GetMyProfile()
            => NewResult(await Mediator.Send(new GetMyProfileQuery()));

        [AllowAnonymous]
        [HttpGet(Router.ProfileRouting.GetPublicProfile)]
        public async Task<IActionResult> GetPublicProfile([FromRoute] int userId)
            => NewResult(await Mediator.Send(new GetPublicProfileQuery(userId)));

        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
        [HttpPut(Router.ProfileRouting.UpdateProfile)]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileCommand command)
            => NewResult(await Mediator.Send(command));

        [AllowAnonymous]
        [HttpGet(Router.ProfileRouting.ConfirmEmailChange)]
        public async Task<IActionResult> ConfirmEmailChange([FromQuery] int userId, [FromQuery] string newEmail, [FromQuery] string token)
            => NewResult(await Mediator.Send(new ConfirmEmailChangeQuery(userId, newEmail, token)));

        [Authorize]
        [TypeFilter(typeof(StudentFilter), Arguments = new object[] { "" })]
        [HttpPut(Router.ProfileRouting.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
            => NewResult(await Mediator.Send(command));
    }
}