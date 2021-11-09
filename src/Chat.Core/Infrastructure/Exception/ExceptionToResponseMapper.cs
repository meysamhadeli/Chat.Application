using System.Net;
using FluentValidation;

namespace Chat.Core.Infrastructure.Exception
{
    public class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(System.Exception exception)
        {
            return exception switch
            {
                AppException ex => new ExceptionResponse(new {reason = ex.Message},
                    HttpStatusCode.BadRequest),
                ValidationException ex => new ExceptionResponse(new {reason = ex.Message},
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new {code = "error", reason = "There was an error."},
                    HttpStatusCode.InternalServerError)
            };
        }
    }
}