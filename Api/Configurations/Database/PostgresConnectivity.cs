using Data.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Database;
using Shared.Constants;
using Shared.Exceptions;

namespace Api.Configurations.Database
{
    public static class PostgresConnectivity
    {
        public static void PostgresConnectivityViaServer(this WebApplicationBuilder builder)
        {
            DbConnectionSettings? dbConnection = builder.Configuration.GetSection(AppSettingsEnvironments.PostgresConnection).Get<DbConnectionSettings>();
            if (dbConnection is null)
            {
                throw new BadEnvironmentValue("Postgres connection might not be configured");
            }
            if (!dbConnection.IsValid(out string dbErrorMessage)) throw new BadEnvironmentValue(dbErrorMessage);
            builder.Services.AddDbContext<DataDbContext>(options =>
            options.UseNpgsql(dbConnection.ToString()));
        }
    }
}
