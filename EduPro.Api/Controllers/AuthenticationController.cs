using EduPro.Api.Base;
using EduPro.Core.Features.Authentication.Commands.Models;
using EduPro.Core.Features.Authentication.Queries.Models;
using EduPro.Data.AppMetaData;
using Microsoft.AspNetCore.Mvc;

namespace EduPro.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        [HttpPost(Router.Authentication.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPost(Router.Authentication.SignIn)]
        public async Task<IActionResult> SignIn([FromForm] SignInCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpGet(Router.Authentication.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [HttpPost(Router.Authentication.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPost(Router.Authentication.SendResetPasswordCode)]
        public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpGet(Router.Authentication.ConfirmResetPassword)]
        public async Task<IActionResult> ConfirmResetPassword([FromQuery] ConfirmResetPasswordQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [HttpPost(Router.Authentication.ResetPassword)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
        [HttpPost(Router.Authentication.GoogleSignIn)]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }

}