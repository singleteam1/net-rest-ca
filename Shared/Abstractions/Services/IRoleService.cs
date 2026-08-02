using Shared.Abstractions.Auth;
using Shared.Dtos.Role;

namespace Shared.Abstractions.Services
{
    public interface IRoleService
    {
        public IQueryable<DbRole> Roles { get; }
        public Task<IList<string>> MyRoles(string email);
        public List<string> AllRoles();
        public Task<DbRole> CreateRoleAsync(RoleDto role);
        public Task<bool> AssignRoleAsync(string roleName, string email);
        public Task<bool> DeleteRoleAsync(RoleDto role);
    }
}
