namespace Shared.Common.Configurations
{
    public class MasterAccount : SettingsModelValidation
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Username { get; set; }

        public override bool IsValid(out string? errorMessage)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Username))
            {
                errorMessage = "Master account data is missing field(s)";
                return false;
            }
            errorMessage = null;
            return true;
        }
    }
}
