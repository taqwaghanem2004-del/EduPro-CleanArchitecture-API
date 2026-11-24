using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Profile.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using EduPro.Data.Enums;
using EduPro.Data.Results.Profile;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Profile.Queries.Handlers
{
    public class ProfileQueryHandler : ResponseHandler,
        IRequestHandler<GetMyProfileQuery, Response<GetMyProfileResult>>,
        IRequestHandler<GetPublicProfileQuery, Response<GetPublicProfileResult>>,
        IRequestHandler<ConfirmEmailChangeQuery, Response<string>>


    {
        #region Fields
        private readonly ICurrentUserService _currentUserService;
        private readonly IEduFileService _eduFileService;
        private readonly ICollectionService _collectionService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IApplicationUserService _applicationUserService;

        #endregion

        #region Constructors
        public ProfileQueryHandler(
            ICurrentUserService currentUserService,
            IEduFileService eduFileService,
            ICollectionService collectionService,
            UserManager<User> userManager,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            IApplicationUserService applicationUserService) : base(localizer)
        {
            _currentUserService = currentUserService;
            _eduFileService = eduFileService;
            _collectionService = collectionService;
            _userManager = userManager;
            _mapper = mapper;
            _localizer = localizer;
            _applicationUserService = applicationUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<GetMyProfileResult>> Handle(
            GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound<GetMyProfileResult>();

            var files = await _eduFileService.GetMyFilesAsync(userId);
            var collections = await _collectionService.GetProfileCollectionsByUserIdAsync(userId);

            var result = new GetMyProfileResult
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                TotalFiles = files.Count,
                TotalDownloads = files.Sum(x => x.DownloadCount),
                CollectionsCount = collections.Count,
                Files = _mapper.Map<List<MyFileResult>>(files),
                Collections = _mapper.Map<List<MyCollectionResult>>(collections)
            };

            return Success(result);
        }
        public async Task<Response<GetPublicProfileResult>> Handle(
    GetPublicProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<GetPublicProfileResult>();

            var files = await _eduFileService.GetMyFilesAsync(request.UserId);

            var collections = await _collectionService.GetProfileCollectionsByUserIdAsync(request.UserId);

            var approvedFiles = files
                .Where(x => x.Status == EduFileStatus.Approved && x.IsPublished)
                .ToList();

            var publishedCollections = collections
                .Where(x => x.IsPublished)
                .ToList();

            var result = new GetPublicProfileResult
            {
                Id = user.Id,
                FullName = user.FullName,
                ProfilePicture = user.ProfilePicture,
                TotalFiles = approvedFiles.Count,
                TotalDownloads = approvedFiles.Sum(x => x.DownloadCount),
                CollectionsCount = publishedCollections.Count,
                Files = _mapper.Map<List<MyFileResult>>(approvedFiles),
                Collections = _mapper.Map<List<MyCollectionResult>>(publishedCollections)
            };

            return Success(result);
        }

        public async Task<Response<string>> Handle(
    ConfirmEmailChangeQuery request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserService.ConfirmEmailChangeAsync(
                request.UserId, request.NewEmail, request.Token);

            return result switch
            {
                "NotFound" => NotFound<string>(),
                "FailedToChangeEmail" => BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]),
                _ => Success<string>(_localizer[SharedResourcesKeys.Success])
            };
        }

        #endregion
    }
}