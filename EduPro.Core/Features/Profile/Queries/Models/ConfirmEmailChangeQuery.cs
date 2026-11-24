using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Profile.Queries.Models
{
    public class ConfirmEmailChangeQuery : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }
        public string Token { get; set; }

        public ConfirmEmailChangeQuery(int userId, string newEmail, string token)
        {
            UserId = userId;
            NewEmail = newEmail;
            Token = token;
        }
    }
}
