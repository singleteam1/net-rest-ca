using Shared.Common.Configurations;

namespace Shared.Common.Database
{
    public class DbConnectionSettings : SettingsModelValidation
    {
        public required string Server { get; set; }
        public required int Port { get; set; }
        public required string DatabaseName { get; set; }
        public required string UserId { get; set; }
        public required string Password { get; set; }

        public override bool IsValid(out string? errorMessage)
        {
            if (string.IsNullOrEmpty(Server) || Port < 0 || string.IsNullOrEmpty(DatabaseName) || string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Password))
            {
                errorMessage = "DbConnectionSettings is missing field(s)";
                return false;
            }
            errorMessage = null;
            return true;
        }

        public override string ToString()
        {
            return $"Server={this.Server};Port={this.Port};Database={this.DatabaseName};User Id={this.UserId};Password={this.Password};";
        }
    }
}
