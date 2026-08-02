namespace Shared.Common.Configurations
{
    public abstract class SettingsModelValidation
    { 
        public abstract bool IsValid(out string? errorMessage);
    }
}
