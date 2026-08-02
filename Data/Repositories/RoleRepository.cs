using Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Auth;
using Shared.Abstractions.Repositories;

namespace Data.Repositories
{
    public class RoleRepository(RoleManager<DbRole> roleManager, UserManager<UserEntity> accountManager) : IRoleRepository
    {
        public RoleManager<DbRole> _roleManager { get; } = roleManager;
        private readonly UserManager<UserEntity> _accountManager = accountManager;

        public async Task<IdentityResult> AddRoleToUserAsync(UserEntity user, string roleName)
        {
            return await _accountManager.AddToRoleAsync(user, roleName);
        }
    }
}
