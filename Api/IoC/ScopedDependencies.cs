using Data.Repositories;
using Data.Repositories.Common;
using FluentValidation;
using Shared.Abstractions.Common;
using Shared.Abstractions.Repositories;
using Shared.Dtos.Auth;
using Shared.Validations.Auth;
using System;
using Shared.Abstractions.Services;
using Usecase.Services.Auth;
using Usecase.Services.Users;

namespace Api.IoC
{
    public static class ScopedDependencies
    {
        public static void AddIoCScopes(this WebApplicationBuilder builder)
        {
            #region IoC Repository Pattern Dependencies
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            builder.Services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            #endregion

            #region IoC Services Dependencies
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            #endregion

            #region IoC Fluent Validations
            builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
            #endregion
        }
    }
}
