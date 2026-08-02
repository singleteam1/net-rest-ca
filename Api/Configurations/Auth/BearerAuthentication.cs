using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Common.Configurations;
using Shared.Exceptions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Api.Configurations.Auth
{
    public static class BearerAuthentication
    {
        public static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, JwtEnvironment _env)
        {
            builder.AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = _env.Issuer,
                    ValidateAudience = false,
                    ValidAudience = _env.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_env.PrivateKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                cfg.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"🔴 Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("✅ Token validated successfully!");
                        return Task.CompletedTask;
                    }
                };
            });
            return builder;
        }

        /// <summary>
        /// Option for swagger, Obsolete for .NET 9
        /// </summary>
        ///
        /// <param name="options"></param>
        public static void AddSwaggerSecurityDefiniton(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter access token in value"
            });
        }

        /// <summary>
        /// Option for swagger, Obsolete for .NET 9
        /// </summary>
        ///
        /// <param name="options"></param>
        public static void AddSwaggerSecurityRequirements(this SwaggerGenOptions options)
        {
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
                    },
                    new List<string>()
                }
            });
        }
    }
}
