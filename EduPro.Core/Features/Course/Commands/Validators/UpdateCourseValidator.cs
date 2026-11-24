using EduPro.Core.Features.Course.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Service.Abstracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Course.Commands.Validators
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Constructors
        public UpdateCourseValidator(IStringLocalizer<SharedResources> stringLocalizer,
                                     ICourseService courseService,
                                     ICategoryService categoryService)
        {
            _stringLocalizer = stringLocalizer;
            _courseService = courseService;
            _categoryService = categoryService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (model, Key, CancellationToken) => !await _courseService.IsCourseNameExistExcludeSelf(Key, model.Id))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.CategoryId)
                .MustAsync(async (Key, CancellationToken) => await _categoryService.IsCategoryIdExist(Key))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.IsNotExist]);
        }
        #endregion
    }
}