using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Webion.AspNetCore;

namespace Webion.Stargaze.Api.Config;

public sealed class ControllersConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }

    public void Use(WebApplication app)
    {
        app.MapControllers();
    }
}