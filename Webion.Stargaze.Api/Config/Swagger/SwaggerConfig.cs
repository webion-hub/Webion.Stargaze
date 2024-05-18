using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config.Swagger;

public sealed class SwaggerConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<ApiVersioningOptions>();
        builder.Services.ConfigureOptions<BearerSecurityOptions>();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            
            options.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "duration", Example = new OpenApiString(Guid.NewGuid().ToString()), });
            options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "duration", Example = new OpenApiString(DateTime.UtcNow.ToString("O")), });
            options.MapType<DateTimeOffset>(() => new OpenApiSchema { Type = "string", Format = "duration", Example = new OpenApiString(DateTimeOffset.UtcNow.ToString("O")), });
            options.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Format = "duration", Example = new OpenApiString("00:45:00"), });
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public void Use(WebApplication app)
    {
        app.UseSwagger();

        app.MapGet("/{version}/_docs", (string version) => Results.Content(
            $$"""
                <!doctype html>
                <html>
                <head>
                    <title>Stargaze Api Reference -- {{version}}</title>
                    <meta charset="utf-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1" />
                </head>
                <body>
                    <script id="api-reference" data-url="/swagger/{{version}}/swagger.json"></script>
                    <script>
                        var configuration = {
                            theme: 'purple',
                        }

                        document.getElementById('api-reference').dataset.configuration = JSON.stringify(configuration)
                    </script>
                    <script src="https://cdn.jsdelivr.net/npm/@scalar/api-reference"></script>
                </body>
                </html>
            """, "text/html"));
    }
}