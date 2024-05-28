using System.Text.Json.Serialization;
using Webion.AspNetCore;
using Webion.Stargaze.Core.Converters;

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
                x.JsonSerializerOptions.Converters.Add(new ClickUpObjectIdConverter());
            });
    }

    public void Use(WebApplication app)
    {
        app.MapControllers();
    }
}