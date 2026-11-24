using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Profile.Commands.Models
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
