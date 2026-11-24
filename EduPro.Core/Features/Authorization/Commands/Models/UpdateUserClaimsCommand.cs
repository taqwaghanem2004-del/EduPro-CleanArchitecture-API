using EduPro.Core.Bases;
using EduPro.Data.Requests;
using MediatR;

namespace EduPro.Core.Features.Authorization.Commands.Models
{
    public class UpdateUserClaimsCommand : UpdateUserClaimsRequest, IRequest<Response<string>>
    {
    }
}