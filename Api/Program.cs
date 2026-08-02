using Api.Configurations.Auth;
using Api.Configurations.Cors;
using Api.Configurations.Database;
using Api.Configurations.Identity;
using Api.IoC;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shared.Common.Configurations;
using Shared.Common.Cors;
using Shared.Constants;
using Shared.Exceptions;
using System.Reflection;
using Data.Context;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            MasterAccount? masterAccount = builder.Configuration.GetSection(AppSettingsEnvironments.MasterAccount).Get<MasterAccount>();
            if (masterAccount is null)
            {
                throw new BadEnvironmentValue("Master account might not be configured");
            }
            if (!masterAccount.IsValid(out string masterAccountErrorMessage)) throw new BadEnvironmentValue(masterAccountErrorMessage);
            JwtEnvironment? jwtEnv = builder.Configuration.GetSection(AppSettingsEnvironments.JwtEnvironment).Get<JwtEnvironment>();
            if (jwtEnv is null)
            {
                throw new BadEnvironmentValue("Jwt environment might not be configured");
            }
            if (!jwtEnv.IsValid(out string jwtEnvErrorMessage)) throw new BadEnvironmentValue(jwtEnvErrorMessage);

            string? assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            if (assemblyName is null)
            {
                throw new BadEnvironmentValue("Assembly name is null");
            }

            builder.Services.AddControllers();
            // .NET 9
            builder.Services.AddOpenApi();

            // Db Connection & Role Identity
            builder.PostgresConnectivityViaServer();
            builder.AddRoleIdentity();

            // JWT Bearer Auth
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtEnv);
            builder.Services.AddAuthorization();



            // IoC
            builder.AddIoCScopes();
            builder.AddUtilsDependencies();
            builder.BindEnvironments();

            // Cors policy
            CorsPolicy? corsPolicy = builder.Configuration.GetSection(AppSettingsEnvironments.CorsPolicy).Get<CorsPolicy>();
            builder.AddPolicy(corsPolicy);

            // Enable FluentValidation automatic validation
            builder.Services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            });


            var app = builder.Build();


            // Cors policy
            app.UseCorsWithPolicy(corsPolicy);

            // Middlewares
            app.UseMiddleware<ExeptionHandlerMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.SeedDb(masterAccount).ConfigureAwait(false);
            app.Run();
        }
    }
}
