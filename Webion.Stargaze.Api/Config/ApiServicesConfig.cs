using Webion.AspNetCore;
using Webion.Stargaze.Services.Extensions;

namespace Qubi.Api.Config;

public sealed class ApiServicesConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.AddServices();
    }

    public void Use(WebApplication app)
    {
    }
}