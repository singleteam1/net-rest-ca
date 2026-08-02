namespace Shared.Common.Auth
{
    public class AuthorizationResponse<TUserEntity>
    {
        public required string RefreshToken { get; set; }
        public required string AccessToken { get; set; }
        public required TUserEntity User { get; set; }
    }
}
