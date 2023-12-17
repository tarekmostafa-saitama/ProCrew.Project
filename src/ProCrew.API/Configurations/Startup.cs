namespace ProCrew.API.Configurations;

internal static class Startup
{
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Configurations";
        var env = builder.Environment;
        builder.Configuration.AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/logging-settings.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/cors-settings.json", false, true)
            .AddEnvironmentVariables();
        return builder;
    }
}