namespace Shared.Dtos.Auth
{
    public class UserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
