using System.Net.Http.Headers;
using Webion.AspNetCore;
using Webion.ClickUp.Api.V2;

namespace Webion.Stargaze.Api.Config;

public sealed class ClickUpApiConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services
            .AddClickUpApi()
            .ConfigureHttpClient(x =>
            {
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    builder.Configuration["ClickUp:ApiKey"]!
                );
            });
    }

    public void Use(WebApplication app)
    {
    }
}