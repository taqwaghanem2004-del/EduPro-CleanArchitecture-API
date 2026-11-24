using EduPro.Core.Features.Profile.Commands.Models;
using EduPro.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Profile.Commands.Validators
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateProfileValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        private void ApplyValidationsRules()
        {
            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .When(x => !string.IsNullOrEmpty(x.FullName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.EmailNotValid])
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
