using EduPro.Core.Features.Profile.Commands.Models;
using EduPro.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Profile.Commands.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ChangePasswordValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        private void ApplyValidationsRules()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MinimumLength(8).WithMessage(_localizer[SharedResourcesKeys.PasswordMinLength])
                .Matches(@"[!@#$%^&*(),.?""{|}|<>]")
                .WithMessage(_localizer[SharedResourcesKeys.PasswordRequireSpecialChar]);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);
        }
    }
}
