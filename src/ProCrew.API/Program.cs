using Google.Cloud.Firestore;
using ProCrew.API.Configurations;
using ProCrew.Application;
using ProCrew.Application.Common.Services;
using ProCrew.Infrastructure;
using ProCrew.Infrastructure.Logging;
using ProCrew.Infrastructure.Logging.SerilogSettings;
using ProCrew.Infrastructure.Persistence;
using ProCrew.Infrastructure.Services;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);


    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
        "Configurations/firebase.json");
    builder.Services.AddScoped<IAuditService>(s => new AuditService(
        FirestoreDb.Create("procrew-7a6c7")
    ));

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddControllers();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);


    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddOpenApiDocument(options =>
    {
        options.DocumentName = "v1";
        options.Title = "ProCrew.Project";
        options.Version = "v1";
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        // Initialise and seed database
        using var scope = app.Services.CreateScope();
        var initialiser =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.SeedAsync();

        app.UseOpenApi();
        app.UseSwaggerUi3();
    }
    else
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseInfrastructure(builder.Configuration);
    app.MapControllers();
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

namespace ProCrew.API
{
    public class Program
    {
    }
}