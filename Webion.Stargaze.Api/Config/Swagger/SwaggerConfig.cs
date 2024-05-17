using Webion.AspNetCore;

namespace Qubi.Api.Config.Swagger;

public sealed class SwaggerConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<ApiVersioningOptions>();
        builder.Services.ConfigureOptions<BearerSecurityOptions>();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

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

                        document.getElementById('api-reference').dataset.configuration =
                        JSON.stringify(configuration)
                    </script>
                    <script src="https://cdn.jsdelivr.net/npm/@scalar/api-reference"></script>
                </body>
                </html>
            """, "text/html"));
    }
}