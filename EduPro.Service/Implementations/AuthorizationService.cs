using EduPro.Data.Entities.Identity;
using EduPro.Data.Helpers;
using EduPro.Data.Requests;
using EduPro.Data.Results.Authorization;
using EduPro.Infrastructure.Data;
using EduPro.Service.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace EduPro.Service.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        #endregion

        #region Constructors
        public AuthorizationService(RoleManager<Role> roleManager,
                                    UserManager<User> userManager,
                                    AppDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddRoleAsync(string roleName)
        {
            var identityRole = new Role();
            identityRole.Name = roleName;
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded) return "Success";
            return "Failed";
        }

        public async Task<bool> IsRoleExistByName(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<string> EditRoleAsync(EditRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null) return "notFound";
            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded) return "Success";
            var errors = string.Join("-", result.Errors);
            return errors;
        }

        public async Task<string> DeleteRoleAsync(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return "NotFound";
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users != null && users.Count() > 0) return "Used";
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) return "Success";
            var errors = string.Join("-", result.Errors);
            return errors;
        }

        public async Task<bool> IsRoleExistById(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return false;
            return true;
        }

        public async Task<List<Role>> GetRolesList()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<ManageUserRolesResult> ManageUserRolesData(User user)
        {
            var response = new ManageUserRolesResult();
            var rolesList = new List<UserRoles>();
            var roles = await _roleManager.Roles.ToListAsync();
            response.UserId = user.Id;
            foreach (var role in roles)
            {
                var userRole = new UserRoles();
                userRole.Id = role.Id;
                userRole.Name = role.Name;
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userRole.HasRole = true;
                else
                    userRole.HasRole = false;
                rolesList.Add(userRole);
            }
            response.userRoles = rolesList;
            return response;
        }

        public async Task<string> UpdateUserRoles(UpdateUserRolesRequest request)
        {
            var transact = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null) return "UserIsNull";
                var userRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded) return "FailedToRemoveOldRoles";
                var selectedRoles = request.userRoles.Where(x => x.HasRole == true).Select(x => x.Name);
                var addRolesResult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addRolesResult.Succeeded) return "FailedToAddNewRoles";
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "FailedToUpdateUserRoles";
            }
        }

        public async Task<ManageUserClaimsResult> ManageUserClaimData(User user)
        {
            var response = new ManageUserClaimsResult();
            var userClaimsList = new List<UserClaims>();
            response.UserId = user.Id;
            var userClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in ClaimsStore.claims)
            {
                var userClaim = new UserClaims();
                userClaim.Type = claim.Type;
                if (userClaims.Any(x => x.Type == claim.Type))
                    userClaim.Value = true;
                else
                    userClaim.Value = false;
                userClaimsList.Add(userClaim);
            }
            response.userClaims = userClaimsList;
            return response;
        }

        public async Task<string> UpdateUserClaims(UpdateUserClaimsRequest request)
        {
            var transact = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null) return "UserIsNull";

                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (!isAdmin) return "UserIsNotAdmin";

                var userClaims = await _userManager.GetClaimsAsync(user);
                var removeClaimsResult = await _userManager.RemoveClaimsAsync(user, userClaims);
                if (!removeClaimsResult.Succeeded) return "FailedToRemoveOldClaims";

                var selectedClaims = request.userClaims.Where(x => x.Value == true).ToList();

                if (selectedClaims.Any(x => x.Type == "SuperAdmin"))
                {
                    var allClaimTypes = new[] { "ManageCategories", "ManageCourses", "ManageFiles", "SuperAdmin" };
                    foreach (var claimType in allClaimTypes)
                    {
                        if (!selectedClaims.Any(x => x.Type == claimType))
                            selectedClaims.Add(new UserClaims { Type = claimType, Value = true });
                    }
                }

                var claims = selectedClaims.Select(x => new Claim(x.Type, x.Value.ToString()));
                var addUserClaimResult = await _userManager.AddClaimsAsync(user, claims);
                if (!addUserClaimResult.Succeeded) return "FailedToAddNewClaims";

                await transact.CommitAsync();
                return "Success";
            }
            catch
            {
                await transact.RollbackAsync();
                return "FailedToUpdateClaims";
            }
        }
        public async Task<User> GetUserById(int id)
        {

            return await _userManager.FindByIdAsync(id.ToString());
        }
        #endregion

    }

}
