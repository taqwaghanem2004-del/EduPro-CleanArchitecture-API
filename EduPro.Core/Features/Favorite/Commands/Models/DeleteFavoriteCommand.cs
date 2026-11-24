using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Favorite.Commands.Models
{
    public class DeleteFavoriteCommand : IRequest<Response<string>>
    {
        public int EduFileId { get; set; }
        public DeleteFavoriteCommand(int eduFileId) => EduFileId = eduFileId;
    }
}