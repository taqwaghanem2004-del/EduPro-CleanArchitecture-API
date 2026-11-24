using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Favorite.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Results.Favorite;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Favorite.Queries.Handlers
{
    public class FavoriteQueryHandler : ResponseHandler,
        IRequestHandler<GetFavoriteListQuery, Response<List<GetFavoriteListResult>>>,
       IRequestHandler<GetFavoriteCollectionsQuery, Response<List<GetFavoriteCollectionListResult>>>

    {
        #region Fields
        private readonly IFavoriteService _favoriteService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public FavoriteQueryHandler(IFavoriteService favoriteService,
                                    IMapper mapper,
                                    ICurrentUserService currentUserService,
                                    IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _favoriteService = favoriteService;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<GetFavoriteListResult>>> Handle(GetFavoriteListQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            var result = _mapper.Map<List<GetFavoriteListResult>>(favorites);
            return Success(result);
        }
        public async Task<Response<List<GetFavoriteCollectionListResult>>> Handle(GetFavoriteCollectionsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var favorites = await _favoriteService.GetFavoriteCollectionsByUserIdAsync(userId);
            var result = _mapper.Map<List<GetFavoriteCollectionListResult>>(favorites);
            return Success(result);
        }
        #endregion
    }
}