using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Services;
using Shared.Common.Configurations;
using Shared.Constants;
using Shared.Dtos.Role;
using Shared.Entities;
using Shared.Exceptions;

namespace Data.Context
{
    public static class DbSeed
    {
        public static async Task SeedDb(this IApplicationBuilder app, MasterAccount masterAccount)
        {
            using (var scope =
                   app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
                    context.Database.Migrate();
                    await InitRoles(scope);
                    await InitMasterAccount(scope, masterAccount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
        }

        private static async Task InitRoles(IServiceScope scope)
        {
            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
            if(roleService.Roles.Any())
            {
                return;
            }
            await roleService.CreateRoleAsync(new RoleDto(UserRoleTypes.Customer));
            await roleService.CreateRoleAsync(new RoleDto(UserRoleTypes.Admin));
        }

        private static async Task InitMasterAccount(IServiceScope scope, MasterAccount masterAccount)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
            if (userManager.Users.Any())
            {
                return;
            }

            var masterEntity = new UserEntity()
            {
                Email = masterAccount.Email,
                UserName = masterAccount.Username,
                DisplayName = masterAccount.Username,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,

            };
            try
            {
                var masterCreationResult = await userManager.CreateAsync(masterEntity, masterAccount.Password);
                bool isAssign = await roleService.AssignRoleAsync(UserRoleTypes.Admin, masterEntity.Email);
                if (!isAssign)
                {
                    throw new BadDataException("Role isn't assigned to master account");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
