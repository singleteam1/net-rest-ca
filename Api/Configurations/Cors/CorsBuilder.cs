using Shared.Common.Cors;
using Shared.Exceptions;

namespace Api.Configurations.Cors
{
    public static class CorsBuilder
    {
        public static WebApplicationBuilder AddPolicy(this WebApplicationBuilder builder, CorsPolicy? _corsPolicy)
        {
            if (_corsPolicy is null)
            {
                throw new BadEnvironmentValue("Cors policy might not be configured");
            }
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicy.PolicyName, policy =>
                {
                    policy.WithOrigins(_corsPolicy.AllowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            return builder;
        }

        public static WebApplication UseCorsWithPolicy(this WebApplication app, CorsPolicy? _corsPolicy)
        {
            if (_corsPolicy is null)
            {
                throw new BadEnvironmentValue("Cors policy might not be configured");
            }
            app.UseCors(_corsPolicy.PolicyName);
            return app;
        }
    }
}
