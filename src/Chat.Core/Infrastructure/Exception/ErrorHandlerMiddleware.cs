using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Chat.Core.Infrastructure.Exception
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly IExceptionToResponseMapper _exceptionToResponseMapper;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(IExceptionToResponseMapper exceptionToResponseMapper,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _exceptionToResponseMapper = exceptionToResponseMapper;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, System.Exception exception)
        {
            var response = context.Response;
            context.Response.ContentType = "application/json";

            var exceptionResponse = _exceptionToResponseMapper.Map(exception);
            context.Response.StatusCode = (int) (exceptionResponse?.StatusCode ?? HttpStatusCode.BadRequest);
            if (exceptionResponse is null)
            {
                await context.Response.WriteAsync(string.Empty);
                return;
            }

            var result = JsonSerializer.Serialize(exceptionResponse);
            await response.WriteAsync(result);
        }
    }
}