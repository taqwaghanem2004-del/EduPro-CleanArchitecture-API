using EduPro.Core.Bases;
using EduPro.Data.Results;
using MediatR;

namespace EduPro.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<Response<JwtAuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
