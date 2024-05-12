using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class SwaggerConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
    }

    public void Use(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}