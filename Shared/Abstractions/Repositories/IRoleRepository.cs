using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Auth;
using Shared.Entities;

namespace Shared.Abstractions.Repositories
{
    public interface IRoleRepository
    {
        public RoleManager<DbRole> _roleManager { get; }
        public Task<IdentityResult> AddRoleToUserAsync(UserEntity user, string roleName);
    }
}
