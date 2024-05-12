using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class CorsConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(o =>
        {
            o.AddDefaultPolicy(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowCredentials();
                x.SetIsOriginAllowed(_ => true);
            });
        });
    }

    public void Use(WebApplication app)
    {
        app.UseCors();
    }
}