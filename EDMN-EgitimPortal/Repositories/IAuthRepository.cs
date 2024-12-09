using EgitimPortalFinal.Models;
using EgitimPortalFinal.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EgitimPortalFinal.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<SignInResult> LoginUserAsync(LoginModel model);
        Task LogoutUserAsync();
        Task<List<AppUser>> GetUsersAsync();
        Task<List<AppRole>> GetRolesAsync();
        Task<IdentityResult> AddRoleAsync(AppRole model);
        Task<IdentityResult> AddRoleToUserAsync(string userId, List<string> selectedRoles);
        Task<AppUser> FindUserByIdAsync(string userId);
        Task<AppRole> FindRoleByNameAsync(string roleName);
        Task AddToRoleAsync(AppUser user, string roleName);
    }
}
