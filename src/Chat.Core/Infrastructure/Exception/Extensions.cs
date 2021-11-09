using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Core.Infrastructure.Exception
{
    public static class Extensions
    {
        public static IServiceCollection AddErrorHandler<T>(this IServiceCollection services)
            where T : class, IExceptionToResponseMapper
        {
            services.AddTransient<ErrorHandlerMiddleware>();
            services.AddSingleton<IExceptionToResponseMapper, T>();

            return services;
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}