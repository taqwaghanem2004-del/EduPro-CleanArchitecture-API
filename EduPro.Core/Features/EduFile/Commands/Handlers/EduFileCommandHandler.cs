using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.EduFile.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using EduPro.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.EduFile.Commands.Handlers
{
    public class EduFileCommandHandler : ResponseHandler,
        IRequestHandler<UploadEduFileCommand, Response<string>>,
        IRequestHandler<ApproveEduFileCommand, Response<string>>,
        IRequestHandler<RejectEduFileCommand, Response<string>>,
        IRequestHandler<DeleteEduFileCommand, Response<string>>,
        IRequestHandler<DownloadEduFileCommand, Response<string>>,
        IRequestHandler<UpdateMyFileCommand, Response<string>>,
        IRequestHandler<PublishEduFileCommand, Response<string>>,
        IRequestHandler<UnpublishEduFileCommand, Response<string>>,
        IRequestHandler<AdminDeleteEduFileCommand, Response<string>>,
        IRequestHandler<AskQuestionCommand, Response<string>>
    {
        private readonly IEduFileService _eduFileService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IUserActivityService _userActivityService;
        private readonly IAiService _aiService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EduFileCommandHandler(IEduFileService eduFileService,
                                     IMapper mapper,
                                     ICurrentUserService currentUserService,
                                     IStringLocalizer<SharedResources> stringLocalizer,
                                     IUserActivityService userActivityService,
                                     IAiService aiService,
                                     IWebHostEnvironment webHostEnvironment) : base(stringLocalizer)
        {
            _eduFileService = eduFileService;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
            _userActivityService = userActivityService;
            _aiService = aiService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response<string>> Handle(UploadEduFileCommand request, CancellationToken cancellationToken)
        {
            var eduFile = _mapper.Map<Data.Entities.EduFile>(request);
            eduFile.UploadedByUserId = _currentUserService.GetUserId();
            eduFile.CourseId = request.CourseId;
            eduFile.RequestedCourseName = request.RequestedCourseName;
            eduFile.RequestedCategoryName = request.RequestedCategoryName;
            var result = await _eduFileService.UploadEduFileAsync(eduFile, request.File);
            if (result == "Success")
                await _userActivityService.LogAsync(eduFile.UploadedByUserId,
                    ActivityType.UploadFile, $"Uploaded file: {request.Title}");
            return result switch
            {
                "InvalidFileType" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidFileType]),
                "FileTooLargeVideo" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FileTooLargeVideo]),
                "FileTooLargeDocument" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FileTooLargeDocument]),
                "EmptyFile" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmptyFile]),
                "FailedToUploadFile" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUploadFile]),
                _ => Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(ApproveEduFileCommand request, CancellationToken cancellationToken)
        {
            var result = await _eduFileService.ApproveEduFileAsync(request.Id);
            if (result == "NotFound")
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var userId = _currentUserService.GetUserId();
            await _userActivityService.LogAsync(userId,
                ActivityType.ApproveFile, $"Approved file ID: {request.Id}");


            var eduFile = await _eduFileService.GetEduFileByIdAsync(request.Id);
            if (eduFile != null)
            {
                var absolutePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    eduFile.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                await _aiService.UploadAndProcessFileAsync(
                    eduFile.Id,
                    absolutePath,
                    Path.GetFileName(eduFile.FilePath));
            }

            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(RejectEduFileCommand request, CancellationToken cancellationToken)
        {
            var result = await _eduFileService.RejectEduFileAsync(request.Id);
            if (result == "NotFound")
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            var userId = _currentUserService.GetUserId();
            await _userActivityService.LogAsync(userId,
                ActivityType.RejectFile, $"Rejected file ID: {request.Id}");
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(DeleteEduFileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _eduFileService.DeleteEduFileAsync(request.Id, userId);
            if (result == "Success")
                await _userActivityService.LogAsync(userId,
                    ActivityType.DeleteFile, $"Deleted file ID: {request.Id}");
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "AlreadyDeleted" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.BadRequest]),
                _ => Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(DownloadEduFileCommand request, CancellationToken cancellationToken)
        {
            var result = await _eduFileService.DownloadEduFileAsync(request.Id);
            if (result == "NotFound")
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var userId = _currentUserService.GetUserId();
            if (userId > 0)
            {
                await _userActivityService.LogAsync(userId,
                    ActivityType.DownloadFile, $"Downloaded file ID: {request.Id}");
            }

            return Success(result);
        }

        public async Task<Response<string>> Handle(UpdateMyFileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _eduFileService.UpdateMyFileAsync(
                request.Id, userId, request.Title, request.Description,
                request.CourseId, request.FileType);
            if (result == "NotFound") return NotFound<string>();
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(PublishEduFileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _eduFileService.PublishEduFileAsync(request.Id, userId);
            if (result == "Success")
            {
                await _userActivityService.LogAsync(
                    userId,
                    ActivityType.PublishFile,
                    $"Published file ID: {request.Id}");
            }
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "NotApproved" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.BadRequest]),
                "AlreadyPublished" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AlreadyExist]),
                _ => Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(UnpublishEduFileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var result = await _eduFileService.UnpublishEduFileAsync(request.Id, userId);
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "AlreadyUnpublished" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.BadRequest]),
                _ => Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(AdminDeleteEduFileCommand request, CancellationToken cancellationToken)
        {
            var result = await _eduFileService.AdminDeleteEduFileAsync(request.Id);
            if (result == "Success")
            {
                var userId = _currentUserService.GetUserId();
                await _userActivityService.LogAsync(
                    userId,
                    ActivityType.AdminHideFile,
                    $"Admin hid file ID: {request.Id}");
            }
            return result switch
            {
                "NotFound" => NotFound<string>(),
                "AlreadyDeleted" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.BadRequest]),
                _ => Success<string>(_stringLocalizer[SharedResourcesKeys.Success])
            };
        }

        public async Task<Response<string>> Handle(AskQuestionCommand request, CancellationToken cancellationToken)
        {
            var answer = await _aiService.AskQuestionAsync(request.FileId, request.Question);
            return Success(answer);
        }
    }
}