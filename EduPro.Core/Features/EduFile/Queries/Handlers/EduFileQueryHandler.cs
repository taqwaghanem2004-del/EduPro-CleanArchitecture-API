using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.EduFile.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Data.Results.Collection;
using EduPro.Data.Results.EduFile;
using EduPro.Infrastructure.Abstracts;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.EduFile.Queries.Handlers
{
    public class EduFileQueryHandler : ResponseHandler,
        IRequestHandler<GetEduFileListQuery, Response<List<GetEduFileListResult>>>,
        IRequestHandler<GetEduFileByIdQuery, Response<GetEduFileByIdResult>>,
        IRequestHandler<GetEduFileByCourseIdQuery, Response<List<GetEduFileListResult>>>,
        IRequestHandler<SearchEduFileQuery, Response<SearchResult>>,
        IRequestHandler<GetMyFilesQuery, Response<List<GetMyFilesResult>>>
    {
        private readonly IEduFileService _eduFileService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IUserActivityService _userActivityService;

        public EduFileQueryHandler(IEduFileService eduFileService,
                                   IMapper mapper,
                                   IStringLocalizer<SharedResources> stringLocalizer,
                                   ICurrentUserService currentUserService,
                                   ICollectionRepository collectionRepository,
                                   IUserActivityService userActivityService) : base(stringLocalizer)
        {
            _eduFileService = eduFileService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _currentUserService = currentUserService;
            _collectionRepository = collectionRepository;
            _userActivityService = userActivityService;
        }

        public async Task<Response<List<GetEduFileListResult>>> Handle(
            GetEduFileListQuery request, CancellationToken cancellationToken)
        {
            var eduFiles = await _eduFileService.GetEduFilesByStatusAsync(request.Status);
            var result = _mapper.Map<List<GetEduFileListResult>>(eduFiles);
            return Success(result);
        }

        public async Task<Response<GetEduFileByIdResult>> Handle(
            GetEduFileByIdQuery request, CancellationToken cancellationToken)
        {
            var eduFile = await _eduFileService.GetEduFileByIdAsync(request.Id);
            if (eduFile == null)
                return NotFound<GetEduFileByIdResult>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            var result = _mapper.Map<GetEduFileByIdResult>(eduFile);
            return Success(result);
        }

        public async Task<Response<List<GetEduFileListResult>>> Handle(
            GetEduFileByCourseIdQuery request, CancellationToken cancellationToken)
        {
            var eduFiles = await _eduFileService.GetEduFilesByCourseIdAsync(request.CourseId);
            var result = _mapper.Map<List<GetEduFileListResult>>(eduFiles);
            return Success(result);
        }

        public async Task<Response<SearchResult>> Handle(
            SearchEduFileQuery request, CancellationToken cancellationToken)
        {
            var eduFiles = await _eduFileService.SearchEduFilesAsync(
                request.Keyword,
                request.CategoryId,
                request.CourseId,
                request.FileType,
                request.OrderBy,
                request.IsDescending);

            var collections = await _collectionRepository.SearchCollectionsAsync(
                request.Keyword,
                request.CategoryId,
                request.CourseId,
                request.OrderBy,
                request.IsDescending);

            var userId = _currentUserService.GetUserId();
            if (userId > 0)
                await _userActivityService.LogAsync(userId,
                    ActivityType.Search, $"Searched for: {request.Keyword}");

            var result = new SearchResult
            {
                Files = _mapper.Map<List<GetEduFileListResult>>(eduFiles),
                Collections = _mapper.Map<List<GetCollectionListResult>>(collections)
            };

            return Success(result);
        }

        public async Task<Response<List<GetMyFilesResult>>> Handle(
            GetMyFilesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var files = await _eduFileService.GetMyFilesAsync(userId);
            var result = _mapper.Map<List<GetMyFilesResult>>(files);
            return Success(result);
        }
    }
}