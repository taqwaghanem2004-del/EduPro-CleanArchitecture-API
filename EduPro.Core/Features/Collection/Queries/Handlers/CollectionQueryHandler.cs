using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Collection.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Results.Collection;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Collection.Queries.Handlers
{
    public class CollectionQueryHandler : ResponseHandler,
        IRequestHandler<GetCollectionByIdQuery, Response<GetCollectionByIdResult>>,
        IRequestHandler<GetPublicCollectionsQuery, Response<List<GetCollectionListResult>>>,
        IRequestHandler<GetProfileCollectionsQuery, Response<List<GetCollectionListResult>>>,
         IRequestHandler<GetLibraryCollectionsQuery, Response<List<GetCollectionListResult>>>,
        IRequestHandler<GetCollectionByCourseIdQuery, Response<List<GetCollectionListResult>>>



    {
        #region Fields
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public CollectionQueryHandler(ICollectionService collectionService,
                                       ICurrentUserService currentUserService,
                                       IStringLocalizer<SharedResources> localizer,
                                       IMapper mapper) : base(localizer)
        {
            _collectionService = collectionService;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions


        public async Task<Response<GetCollectionByIdResult>> Handle(GetCollectionByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var collection = await _collectionService.GetCollectionByIdAsync(request.Id, userId);
            if (collection == null) return NotFound<GetCollectionByIdResult>();
            var result = _mapper.Map<GetCollectionByIdResult>(collection);
            return Success(result);
        }
        public async Task<Response<List<GetCollectionListResult>>> Handle(GetPublicCollectionsQuery request, CancellationToken cancellationToken)
        {
            var collections = await _collectionService.GetPublicCollectionsAsync();
            var result = _mapper.Map<List<GetCollectionListResult>>(collections);
            return Success(result);
        }
        public async Task<Response<List<GetCollectionListResult>>> Handle(
    GetProfileCollectionsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var collections = await _collectionService.GetProfileCollectionsByUserIdAsync(userId);
            var result = _mapper.Map<List<GetCollectionListResult>>(collections);
            return Success(result);
        }

        public async Task<Response<List<GetCollectionListResult>>> Handle(
            GetLibraryCollectionsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var collections = await _collectionService.GetLibraryCollectionsByUserIdAsync(userId);
            var result = _mapper.Map<List<GetCollectionListResult>>(collections);
            return Success(result);
        }
        public async Task<Response<List<GetCollectionListResult>>> Handle(GetCollectionByCourseIdQuery request, CancellationToken cancellationToken)
        {
            var collections = await _collectionService.GetCollectionsByCourseIdAsync(request.CourseId);
            var result = _mapper.Map<List<GetCollectionListResult>>(collections);
            return Success(result);
        }


        #endregion
    }
}
