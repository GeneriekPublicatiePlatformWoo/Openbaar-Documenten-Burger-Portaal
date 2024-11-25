using ODBP.Apis.Odrc;
using ODBP.Config;
using ODBP.Features.Sitemap;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

using var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();

logger.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(logger);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();
    builder.Services.AddHttpClient();
    builder.Services.AddSingleton<IOdrcClientFactory, OdrcClientFactory>();
    builder.Services.AddBaseUri();
    builder.Services.AddOutputCache(x=> x.AddPolicy(OutputCachePolicies.Sitemap, b=> b.Expire(TimeSpan.FromHours(23))));

    var app = builder.Build();

    app.UseSerilogRequestLogging(x=> x.Logger = logger);
    app.UseDefaultFiles();
    app.UseOdbpStaticFiles();

    if (!app.Environment.IsDevelopment())
    {
        app.UseOutputCache();
    }

    app.UseOdbpSecurityHeaders();

    app.MapControllers();
    app.MapHealthChecks("/healthz");
    app.MapFallbackToIndexHtml();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    logger.Write(LogEventLevel.Fatal, ex, "Application terminated unexpectedly");
}

public partial class Program { }
