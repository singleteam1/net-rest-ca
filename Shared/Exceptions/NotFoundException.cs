namespace Shared.Exceptions
{
    public class NotFoundExeption : Exception
    {
        public NotFoundExeption() : base() { }
        public NotFoundExeption(string message) : base(message) { }
        public NotFoundExeption(string message, Exception innerException) : base(message, innerException) { }
    }
}
