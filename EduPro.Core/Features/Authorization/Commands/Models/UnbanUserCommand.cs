using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Authorization.Commands.Models
{
    public class UnbanUserCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public int AdminUserId { get; set; }
    }
}
