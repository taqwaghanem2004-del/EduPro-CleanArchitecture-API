using EduPro.Core.Bases;
using EduPro.Core.Features.Collection.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;


namespace EduPro.Core.Features.Collection.Commands.Handlers
{
    public class CollectionCommandHandler : ResponseHandler,
     IRequestHandler<UpdateCollectionCommand, Response<string>>,
     IRequestHandler<DeleteCollectionCommand, Response<string>>,
     IRequestHandler<AddFileToCollectionCommand, Response<string>>,
     IRequestHandler<RemoveFileFromCollectionCommand, Response<string>>,
     IRequestHandler<ReorderCollectionFilesCommand, Response<string>>,
     IRequestHandler<PublishCollectionCommand, Response<string>>,
     IRequestHandler<AddCollectionToFavoritesCommand, Response<string>>,
     IRequestHandler<RemoveCollectionFromFavoritesCommand, Response<string>>,
     IRequestHandler<CopyCollectionCommand, Response<string>>,
     IRequestHandler<CreateProfileCollectionCommand, Response<string>>,
     IRequestHandler<CreateLibraryCollectionCommand, Response<string>>,
     IRequestHandler<UnpublishCollectionCommand, Response<string>>
    {
        private readonly ICollectionService _collectionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IFavoriteService _favoriteService;
        private readonly IUserActivityService _userActivityService;


        public CollectionCommandHandler(ICollectionService collectionService,
                                         ICurrentUserService currentUserService,
                                         IStringLocalizer<SharedResources> localizer,
                                         IFavoriteService favoriteService,
                                         IUserActivityService userActivityService) : base(localizer)
        {
            _collectionService = collectionService;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _favoriteService = favoriteService;
            _userActivityService = userActivityService;
        }



        public async Task<Response<string>> Handle(UpdateCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.UpdateCollectionAsync(request.Id, userId, request.Name, request.Description);
            if (result == "NotFound") return NotFound<string>();
            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.DeleteCollectionAsync(request.Id, userId);
            if (result == "NotFound") return NotFound<string>();
            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(AddFileToCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.AddFileToCollectionAsync(request.CollectionId, request.EduFileId, userId);
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "FileNotFound" => NotFound<string>(),
                "NotYourFile" => BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]),
                "WrongCourse" => BadRequest<string>(_localizer[SharedResourcesKeys.WrongCourse]),
                "NotInFavorites" => BadRequest<string>(_localizer[SharedResourcesKeys.NotInFavorites]),
                "AlreadyExist" => BadRequest<string>(_localizer[SharedResourcesKeys.AlreadyExist]),
                _ => Success<string>(_localizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(RemoveFileFromCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.RemoveFileFromCollectionAsync(request.CollectionId, request.EduFileId, userId);
            if (result == "NotFound") return NotFound<string>();
            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(ReorderCollectionFilesCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.ReorderFilesAsync(request.CollectionId, userId, request.Files);
            if (result == "NotFound") return NotFound<string>();
            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }
        public async Task<Response<string>> Handle(PublishCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.PublishCollectionAsync(request.CollectionId, userId);
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "EmptyCollection" => BadRequest<string>(_localizer[SharedResourcesKeys.EmptyCollection]),
                "CannotPublishLibrary" => BadRequest<string>(_localizer[SharedResourcesKeys.CannotPublishLibrary]),
                "NoCourse" => BadRequest<string>(_localizer[SharedResourcesKeys.NoCourse]),
                _ => Success<string>(_localizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(AddCollectionToFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _favoriteService.AddCollectionToFavoritesAsync(request.CollectionId, userId);
            if (result == "AlreadyExist") return BadRequest<string>(_localizer[SharedResourcesKeys.AlreadyExist]);
            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(RemoveCollectionFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _favoriteService.RemoveCollectionFromFavoritesAsync(request.CollectionId, userId);
            if (result == "NotFound") return NotFound<string>();
            return Success<string>(_localizer[SharedResourcesKeys.Success]);
        }
        public async Task<Response<string>> Handle(CopyCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.CopyCollectionAsync(request.CollectionId, userId);

            return result switch
            {
                "NotFound" => NotFound<string>(),
                "CannotCopyOwn" => BadRequest<string>(_localizer[SharedResourcesKeys.CannotCopyOwn]),
                _ => Success<string>(_localizer[SharedResourcesKeys.Success])
            };
        }
        public async Task<Response<string>> Handle(CreateProfileCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.CreateProfileCollectionAsync(userId, request.Name, request.Description, request.CourseId);
            if (result == "Success")
            {
                await _userActivityService.LogAsync( 
                    userId,
                    ActivityType.CreateCollection,
                    $"Created profile collection: {request.Name} for course: {request.CourseId}");
            }

            return result == "Success" ? Success<string>(_localizer[SharedResourcesKeys.Success]) : BadRequest<string>(result);
        }
        public async Task<Response<string>> Handle(CreateLibraryCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.CreateLibraryCollectionAsync(userId, request.Name, request.Description);
            if (result == "Success")
            {
                await _userActivityService.LogAsync( 
                    userId,
                    ActivityType.CreateCollection,
                    $"Created library collection: {request.Name}");
            }

            return result == "Success" ? Success<string>(_localizer[SharedResourcesKeys.Success]) : BadRequest<string>(result);
        }
        public async Task<Response<string>> Handle(UnpublishCollectionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _collectionService.UnpublishCollectionAsync(request.CollectionId, userId);
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "AlreadyUnpublished" => BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]),
                _ => Success<string>(_localizer[SharedResourcesKeys.Success])
            };
        }

    }
}
