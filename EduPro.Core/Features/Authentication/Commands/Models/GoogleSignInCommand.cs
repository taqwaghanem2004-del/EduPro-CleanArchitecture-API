using EduPro.Core.Bases;
using EduPro.Data.Results;
using MediatR;

namespace EduPro.Core.Features.Authentication.Commands.Models
{
    public class GoogleSignInCommand : IRequest<Response<JwtAuthResult>>
    {
        public string IdToken { get; set; }
    }
}
