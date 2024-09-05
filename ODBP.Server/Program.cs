using Microsoft.EntityFrameworkCore;
using ODBP.Data;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

using var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information) //logeventlevel information voor Microsoft.AspNetCore.Authentication namespace omdat deze namespace de unauthorizations gooit, voorbeeld: Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler: Information: AuthenticationScheme: Bearer was challenged.
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();

logger.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();

    var connStr = $"Username={builder.Configuration["POSTGRES_USER"]};Password={builder.Configuration["POSTGRES_PASSWORD"]};Host={builder.Configuration["POSTGRES_HOST"]};Database={builder.Configuration["POSTGRES_DB"]};Port={builder.Configuration["POSTGRES_PORT"]}";
    builder.Services.AddDbContext<OdbpDbContext>(opt => opt.UseNpgsql(connStr));

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseDefaultFiles();
    app.UseOdbpStaticFiles();
    app.UseOdbpSecurityHeaders();

    app.MapControllers();
    app.MapHealthChecks("/healthz");
    app.MapFallbackToIndexHtml();

    await using (var scope = app.Services.CreateAsyncScope())
    {
        await scope.ServiceProvider.GetRequiredService<OdbpDbContext>().Database.MigrateAsync();
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    logger.Write(LogEventLevel.Fatal, ex, "Application terminated unexpectedly");
}

public partial class Program { }
