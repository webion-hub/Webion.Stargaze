using Serilog;
using Serilog.Sinks.GoogleCloudLogging;
using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class LoggingConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, services, logger) =>
        {
            logger
                .ReadFrom.Configuration(ctx.Configuration)
                .ReadFrom.Services(services);

            if (builder.Environment.IsDevelopment() || true)
            {
                logger.WriteTo.Console();
            }
            else
            {
                logger.WriteTo.GoogleCloudLogging(new GoogleCloudLoggingSinkOptions
                {
                    ProjectId = builder.Configuration["Logging:GoogleCloud:ProjectId"],
                    ServiceName = builder.Configuration["Logging:ServiceName"],
                    Labels =
                    {
                        ["env"] = builder.Environment.EnvironmentName,
                    },
                });
            }
        });

    }

    public void Use(WebApplication app)
    {
    }
}