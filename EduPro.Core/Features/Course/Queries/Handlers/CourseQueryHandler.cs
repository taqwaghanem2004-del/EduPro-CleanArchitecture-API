using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Course.Queries.Models;
using EduPro.Core.Resources;
using EduPro.Data.Results.Course;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Course.Queries.Handlers
{
    public class CourseQueryHandler : ResponseHandler,
        IRequestHandler<GetCourseListQuery, Response<List<GetCourseListResult>>>,
        IRequestHandler<GetCourseByIdQuery, Response<GetCourseByIdResult>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public CourseQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  ICourseService courseService,
                                  IMapper mapper) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _courseService = courseService;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<GetCourseListResult>>> Handle(GetCourseListQuery request, CancellationToken cancellationToken)
        {
            var courses = await _courseService.GetCoursesByCategoryIdAsync(request.CategoryId);
            var result = _mapper.Map<List<GetCourseListResult>>(courses);
            return Success(result);
        }

        public async Task<Response<GetCourseByIdResult>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseService.GetCourseByIdAsync(request.Id);
            if (course == null) return NotFound<GetCourseByIdResult>();
            var result = _mapper.Map<GetCourseByIdResult>(course);
            return Success(result);
        }
        #endregion
    }
}