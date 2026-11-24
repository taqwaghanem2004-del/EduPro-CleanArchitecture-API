using EduPro.Core.Bases;
using EduPro.Core.Features.Authorization.Commands.Models;
using EduPro.Core.Resources;
using EduPro.Data.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace EduPro.Core.Features.Authorization.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
        IRequestHandler<BanUserCommand, Response<string>>,
        IRequestHandler<UnbanUserCommand, Response<string>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public UserCommandHandler(UserManager<User> userManager,
                                   IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<string>();
            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<string>();
            user.IsBlocked = false;
            await _userManager.UpdateAsync(user);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
