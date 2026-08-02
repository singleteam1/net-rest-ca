using Data.Context;
using Microsoft.AspNetCore.Identity;
using Shared.Abstractions.Auth;
using Shared.Entities;

namespace Api.Configurations.Identity
{
    public static class RoleIdentity
    {
        public static void AddRoleIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<UserEntity, DbRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;

                opt.SignIn.RequireConfirmedPhoneNumber = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<DataDbContext>();
        }
    }
}
