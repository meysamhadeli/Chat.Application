namespace Chat.Core.Infrastructure.Exception
{
    public interface IExceptionToResponseMapper
    {
        public ExceptionResponse Map(System.Exception exception);
    }
}