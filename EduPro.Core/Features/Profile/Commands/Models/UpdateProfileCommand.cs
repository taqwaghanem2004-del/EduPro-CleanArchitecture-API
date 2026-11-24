using EduPro.Core.Bases;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduPro.Core.Features.Profile.Commands.Models
{

    public class UpdateProfileCommand : IRequest<Response<string>>
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
