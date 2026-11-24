using AutoMapper;
using EduPro.Core.Bases;
using EduPro.Core.Features.Category.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Enums;
using EduPro.Service.Abstracts;
using MediatR;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Category.Commands.Handlers
{
    public class CategoryCommandHandler : ResponseHandler,
       IRequestHandler<CreateCategoryCommand, Response<string>>,
       IRequestHandler<UpdateCategoryCommand, Response<string>>,
       IRequestHandler<DeleteCategoryCommand, Response<string>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IUserActivityService _userActivityService;
        #endregion

        #region Constructors
        public CategoryCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                ICategoryService categoryService,
                                IMapper mapper,
                                IUserActivityService userActivityService) 
      : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _userActivityService = userActivityService; 
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<EduPro.Data.Entities.Category>(request);
            var result = await _categoryService.CreateCategoryAsync(category);
            if (result != "Success")
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            await _userActivityService.LogAsync( 
                request.UserId,
                ActivityType.CreateCategory,
                $"Created category: {request.Name}");

            return Created("");
        }

        public async Task<Response<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            var updatedCategory = _mapper.Map(request, category);
            var result = await _categoryService.UpdateCategoryAsync(updatedCategory);
            if (result != "Success")
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            await _userActivityService.LogAsync( 
                request.UserId,
                ActivityType.UpdateCategory,
                $"Updated category ID: {request.Id}");

            return Success<string>(_localizer[SharedResourcesKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            var result = await _categoryService.DeleteCategoryAsync(category);
            if (result != "Success")
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            await _userActivityService.LogAsync(
                request.UserId,
                ActivityType.DeleteCategory,
                $"Deleted category ID: {request.Id}");

            return Deleted<string>();
        }
        #endregion
    }
}
