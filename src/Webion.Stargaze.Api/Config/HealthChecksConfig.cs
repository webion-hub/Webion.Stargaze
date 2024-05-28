using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class HealthChecksConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        
        builder.Services.AddHealthChecks()
            .AddNpgSql(config.GetConnectionString("db")!);
    }

    public void Use(WebApplication app)
    {
        app.UseHealthChecks("/_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });
    }
}