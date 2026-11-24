using EduPro.Core.Features.Category.Commands.Models;
using EduPro.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Category.Commands.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateCategoryValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);
        }
        #endregion
    }
}
