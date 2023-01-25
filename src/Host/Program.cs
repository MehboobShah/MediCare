using FluentValidation.AspNetCore;
using MediCare.Application;
using MediCare.Host.Configurations;
using MediCare.Host.Controllers;
using MediCare.Infrastructure;
using MediCare.Infrastructure.Common;
using Serilog;

[assembly: ApiConventionType(typeof(FSHApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers().AddFluentValidation();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
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