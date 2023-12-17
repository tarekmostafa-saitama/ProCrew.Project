using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProCrew.Infrastructure.Cors;
using ProCrew.Infrastructure.Middlewares;
using ProCrew.Infrastructure.Persistence;
using ProCrew.Infrastructure.Services;

namespace ProCrew.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddServices()
            .AddCorsPolicy(config)
            .AddCorsPolicy(config)
            .AddPersistence(config)
            .AddCustomMiddlewares();
    }


    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
    {
        return builder
            .UseExceptionHandlerMiddleware()
            .UseHttpsRedirection()
            .UseRouting()
            .UseCorsPolicy();
    }
}