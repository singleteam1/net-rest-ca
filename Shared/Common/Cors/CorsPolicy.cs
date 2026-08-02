namespace Shared.Common.Cors
{
    public class CorsPolicy
    {
        public required string PolicyName { get; set; }
        public required string[] AllowedOrigins { get; set; }
    }
}
