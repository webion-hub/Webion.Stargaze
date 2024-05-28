using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class CorsConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        var origins = builder.Configuration
            .GetSection("AllowedOrigins")
            .Get<string[]>()!;
        
        builder.Services.AddCors(o =>
        {
            o.AddDefaultPolicy(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowCredentials();
                x.WithOrigins(origins);
            });
        });
    }

    public void Use(WebApplication app)
    {
        app.UseCors();
    }
}