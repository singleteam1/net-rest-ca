using Shared.Abstractions.Repositories;
using Shared.Abstractions.Services;
using Shared.Dtos.Auth;
using Shared.Dtos.User;
using Shared.Exceptions;

namespace Usecase.Services.Users
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDto> UpdateUser(string email, UpdateUserDto dto)
        {
            var user = await _userRepository._accountManager.FindByEmailAsync(email);
            if (user is null) throw new NotFoundExeption("User not found");
            if (dto.phone is not null) user.PhoneNumber = dto.phone;
            if (dto.display_name is not null) user.DisplayName = dto.display_name;
            if (dto.username is not null) user.UserName = dto.username;

            var updateResult = await _userRepository.UpdateAsync(user);
            return updateResult.ToUserDto();
        }
    }
}
