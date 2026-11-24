using EduPro.Core.Bases;
using EduPro.Core.Features.Favorite.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Favorite.Commands.Handlers
{
    public class FavoriteCommandHandler : ResponseHandler,
        IRequestHandler<AddFavoriteCommand, Response<string>>,
        IRequestHandler<DeleteFavoriteCommand, Response<string>>
    {
        #region Fields
        private readonly IFavoriteService _favoriteService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IUserActivityService _userActivityService;
        #endregion

        #region Constructors
        public FavoriteCommandHandler(IFavoriteService favoriteService,
                                      ICurrentUserService currentUserService,
                                      IStringLocalizer<SharedResources> stringLocalizer,
                                      IUserActivityService userActivityService) : base(stringLocalizer)
        {
            _favoriteService = favoriteService;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
            _userActivityService = userActivityService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _favoriteService.AddFavoriteAsync(userId, request.EduFileId);
            if (result == "Success") 
            {
                await _userActivityService.LogAsync(
                    userId,
                    ActivityType.AddToFavorites,
                    $"Added file ID: {request.EduFileId} to favorites");
            }


            return result switch
            {
                "EduFileNotFound" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]),
                "AlreadyExist" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AlreadyExist]),
                _ => Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(DeleteFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _favoriteService.DeleteFavoriteAsync(userId, request.EduFileId);

            if (result == "NotFound")
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}