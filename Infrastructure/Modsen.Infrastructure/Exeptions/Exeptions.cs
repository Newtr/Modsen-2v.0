namespace Modsen.Infrastructure
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }

        public DatabaseException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    public class NetworkException : Exception
    {
        public NetworkException(string message) : base(message) { }

        public NetworkException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message) : base(message) { }

        public FileNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}