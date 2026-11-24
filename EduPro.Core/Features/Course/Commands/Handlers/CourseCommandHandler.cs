using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Course.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using EduPro.Service.Implementations;
using MediatR;
using Microsoft.Extensions.Localization;


namespace EduPro.Core.Features.Course.Commands.Handlers
{
    public class CourseCommandHandler : ResponseHandler,
        IRequestHandler<CreateCourseCommand, Response<string>>,
        IRequestHandler<UpdateCourseCommand, Response<string>>,
        IRequestHandler<DeleteCourseCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly IEduFileService _eduFileService;
        private readonly IUserActivityService _userActivityService;
        #endregion

        #region Constructors
        public CourseCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    ICourseService courseService,
                                    IMapper mapper,
                                    IEduFileService eduFileService,
                                    IUserActivityService userActivityService
                                    ) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _courseService = courseService;
            _mapper = mapper;
            _eduFileService = eduFileService;
            _userActivityService = userActivityService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = _mapper.Map<EduPro.Data.Entities.Course>(request);
            var result = await _courseService.AddCourseAsync(course);

            if (result != "Success")
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AddFailed]);

            if (request.EduFileId.HasValue)
            {
                var eduFile = await _eduFileService.GetEduFileByIdAsync(request.EduFileId.Value);
                if (eduFile != null)
                {
                    eduFile.CourseId = course.Id;
                    eduFile.RequestedCourseName = null;
                    eduFile.RequestedCategoryName = null;
                    await _eduFileService.UpdateEduFileAsync(eduFile);
                    await _eduFileService.ApproveEduFileAsync(eduFile.Id);
                }
            }

            await _userActivityService.LogAsync( 
                request.UserId,
                ActivityType.CreateCourse,
                $"Created course: {request.Name}");

            return Created("");
        }

        public async Task<Response<string>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseService.GetCourseByIdAsync(request.Id);
            if (course == null) return NotFound<string>();

            var updatedCourse = _mapper.Map(request, course);
            var result = await _courseService.EditCourseAsync(updatedCourse);
            if (result != "Success")
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AddFailed]);

            await _userActivityService.LogAsync( 
                request.UserId,
                ActivityType.UpdateCourse,
                $"Updated course ID: {request.Id}");

            return Success((string)_stringLocalizer[SharedResourcesKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseService.GetCourseByIdAsync(request.Id);
            if (course == null) return NotFound<string>();

            var result = await _courseService.DeleteCourseAsync(course);
            if (result != "Success")
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AddFailed]);

            await _userActivityService.LogAsync( 
                request.UserId,
                ActivityType.DeleteCourse,
                $"Deleted course ID: {request.Id}");

            return Deleted<string>();
        }
        #endregion
    }
}