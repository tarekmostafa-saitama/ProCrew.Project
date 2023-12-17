using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ProCrew.Infrastructure.Middlewares;

public static class Startup
{
    internal static IServiceCollection AddCustomMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<ExceptionMiddleware>();
        return services;
    }


    internal static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}