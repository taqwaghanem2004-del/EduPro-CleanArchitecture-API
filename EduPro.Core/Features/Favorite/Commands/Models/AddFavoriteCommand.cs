using EduPro.Core.Bases;
using MediatR;

namespace EduPro.Core.Features.Favorite.Commands.Models
{
    public class AddFavoriteCommand : IRequest<Response<string>>
    {
        public int EduFileId { get; set; }
        public AddFavoriteCommand(int eduFileId) => EduFileId = eduFileId;
    }
}