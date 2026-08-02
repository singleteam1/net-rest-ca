namespace Shared.Exceptions
{
    public class BadDataException : Exception
    {
        public BadDataException() : base() { }
        public BadDataException(string message) : base(message) { }
        public BadDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
