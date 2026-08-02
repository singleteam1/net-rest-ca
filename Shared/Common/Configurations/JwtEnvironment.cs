namespace Shared.Common.Configurations
{
    public class JwtEnvironment : SettingsModelValidation
    {
        public required string PrivateKey { get; set; }
        public required string TokenValiditySeconds { get; set; }
        public required string RefreshTokenValiditySeconds { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }

        public override bool IsValid(out string? errorMessage)
        {
            if (string.IsNullOrEmpty(PrivateKey) || string.IsNullOrEmpty(TokenValiditySeconds) || string.IsNullOrEmpty(RefreshTokenValiditySeconds) || string.IsNullOrEmpty(Issuer) || string.IsNullOrEmpty(Audience))
            {
                errorMessage = "JwtEnvironment is missing field(s)";
                return false;
            }
            errorMessage = null;
            return true;
        }
    }
}
