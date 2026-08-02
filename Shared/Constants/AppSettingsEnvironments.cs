namespace Shared.Constants
{
    public static class AppSettingsEnvironments
    {
        public static readonly string JwtEnvironment = "Jwt";
        public static readonly string CorsPolicy = "CorsPolicy";
        public static readonly string DatabaseConnections = "DatabaseConnections";
        public static readonly string PostgresConnection = String.Concat(DatabaseConnections, ":", "Postgres");
        public static readonly string MasterAccount = "MasterAccount";
    }
}
