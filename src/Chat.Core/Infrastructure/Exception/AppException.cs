namespace Chat.Core.Infrastructure.Exception
{
    public class AppException : System.Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}