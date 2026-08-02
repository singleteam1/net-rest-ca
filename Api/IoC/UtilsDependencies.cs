using Data.Repositories.Common;
using Shared.Abstractions.Common;
using Usecase.Utils.Jwt;

namespace Api.IoC
{
    public static class UtilsDependencies
    {
        public static void AddUtilsDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IJwtUtility, JwtUtility>();
        }
    }
}
