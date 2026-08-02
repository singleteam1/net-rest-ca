namespace Shared.Exceptions
{
    public class BadEnvironmentValue : Exception
    {
        public BadEnvironmentValue() : base() { }
        public BadEnvironmentValue(string message) : base(message) { }
        public BadEnvironmentValue(string message, Exception innerException) : base(message, innerException) { }
    }
}
