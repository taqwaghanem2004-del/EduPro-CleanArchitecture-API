using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Authentication.Commands.Models
{
    public class RegisterCommand : IRequest<Response<string>>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
