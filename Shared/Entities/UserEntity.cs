using Shared.Abstractions.Common;
using Shared.Dtos.Auth;

namespace Shared.Entities
{
    public class UserEntity : IBaseIndentityUser
    {
        public string DisplayName { get; set; }
        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiryTime { get; set; } = null;

        public static UserDto ToUserDto(UserEntity entity)
        {
            return new UserDto()
            {
                DisplayName = entity.DisplayName,
                Username = entity.UserName,
                Email = entity.Email ?? "no-email",
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public UserDto ToUserDto() 
        {
            return UserEntity.ToUserDto(this);
        }
    }
}
