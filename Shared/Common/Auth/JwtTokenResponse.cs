namespace Shared.Common.Auth
{
    public class JwtTokenResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
