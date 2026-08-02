using Shared.Abstractions.Services;
using Shared.Abstractions.Auth;
using Shared.Abstractions.Repositories;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Dtos.Role;

namespace Usecase.Services.Auth
{
    public class RoleService(IRoleRepository roleRepository, IUserRepository userRepository) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUserRepository _userRepo = userRepository;
        IQueryable<DbRole> IRoleService.Roles => roleRepository._roleManager.Roles;

        public async Task<IList<string>> MyRoles(string email)
        {
            var user = await _userRepo._accountManager.FindByEmailAsync(email);
            if (user is null) throw new NotFoundExeption("User not found");
            return await _userRepo._accountManager.GetRolesAsync(user);
        }

        public List<string> AllRoles()
        {
            var roles = _roleRepository._roleManager.Roles.Select(s => s.Name).ToList();
            if (roles is null || !roles.Any())
            {
                throw new NotFoundExeption("Roles not found");
            }
            return roles as List<string>;
        }

        public async Task<DbRole> CreateRoleAsync(RoleDto role)
        {
            DbRole dbRole = new()
            {
                Name = role.roleName
            };
            var newRole = await _roleRepository._roleManager.CreateAsync(dbRole);
            if (!newRole.Succeeded)
            {
                throw new BadDataException("Role is not created");
            }

            return dbRole;
        }

        public async Task<bool> AssignRoleAsync(string roleName, string email)
        {
            var user = await _userRepo._accountManager.FindByEmailAsync(email);
            if (user is null) throw new NotFoundExeption("User not found");
            var addToUserRole = await _roleRepository.AddRoleToUserAsync(user, roleName);
            if (!addToUserRole.Succeeded)
            {
                throw new BadDataException("Role is not assigned to appropriate user");
            }

            return true;
        }

        public async Task<bool> DeleteRoleAsync(RoleDto role)
        {
            DbRole findRole = await _roleRepository._roleManager.FindByNameAsync(role.roleName);
            if (findRole is null)
            {
                throw new NotFoundExeption("Role not found");
            }

            if (findRole.Name == UserRoleTypes.Admin || findRole.Name == UserRoleTypes.Customer)
            {
                // Hide name of roles
                throw new NotFoundExeption("Role not found");
            }
            var deletedRole = await _roleRepository._roleManager.DeleteAsync(findRole);
            if (!deletedRole.Succeeded)
            {
               throw new BadDataException($"Role is not deleted, {string.Join(",", deletedRole.Errors.Select(s => s.Description))}");
            }
            return true;
        }
    }
}
