using Shared.Common.Auth;
using Shared.Dtos.Auth;

namespace Shared.Abstractions.Services
{
    public interface IAuthService
    {
        public Task<UserDto> Me(string email);
        public Task<AuthorizationResponse<UserDto>> Register(RegisterDto dto);
        public Task<AuthorizationResponse<UserDto>> Login(LoginDto dto);
        public Task<AuthorizationResponse<UserDto>> RefreshToken(RefreshTokenDto dto);
        public Task<bool> Logout(string email);
    }
}
