using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class AuthZConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();   
    }

    public void Use(WebApplication app)
    {
        app.UseAuthorization();
    }
}