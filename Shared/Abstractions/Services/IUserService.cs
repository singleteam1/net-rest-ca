using Shared.Dtos.Auth;
using Shared.Dtos.User;

namespace Shared.Abstractions.Services
{
    public interface IUserService
    {
        public Task<UserDto> UpdateUser(string email, UpdateUserDto dto);
    }
}
