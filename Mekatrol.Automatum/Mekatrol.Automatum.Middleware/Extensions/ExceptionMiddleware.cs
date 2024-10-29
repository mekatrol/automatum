using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mekatrol.Automatum.Middleware.Extensions;

public static class ExceptionMiddleware
{
    public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlerMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
