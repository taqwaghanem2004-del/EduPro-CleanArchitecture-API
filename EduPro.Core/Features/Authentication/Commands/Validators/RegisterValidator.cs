using EduPro.Core.Features.Authentication.Commands.Models;
using EduPro.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Authentication.Commands.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RegisterValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        private void ApplyValidationsRules()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.EmailNotValid]);


            RuleFor(x => x.Password)
           .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
             .MinimumLength(8).WithMessage(_localizer[SharedResourcesKeys.PasswordMinLength])
              .Matches(@"[!@#$%^&*(),.?""{|}|<>]")
              .WithMessage(_localizer[SharedResourcesKeys.PasswordRequireSpecialChar]);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);
        }
    }
}
