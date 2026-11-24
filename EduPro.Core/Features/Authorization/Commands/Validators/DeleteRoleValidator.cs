using EduPro.Core.Features.Authorization.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Service.Abstracts;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Authorization.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public DeleteRoleValidator(IStringLocalizer<SharedResources> stringLocalizer,
                                   IAuthorizationService authorizationService)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
            ApplyValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }
        #endregion
    }
}