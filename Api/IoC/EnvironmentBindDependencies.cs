using Shared.Common.Configurations;
using Shared.Constants;

namespace Api.IoC
{
    public static class EnvironmentBindDependencies
    {
        public static void BindEnvironments(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtEnvironment>(builder.Configuration.GetSection(AppSettingsEnvironments.JwtEnvironment));
            builder.Services.Configure<MasterAccount>(builder.Configuration.GetSection(AppSettingsEnvironments.MasterAccount));
        }
    }
}
