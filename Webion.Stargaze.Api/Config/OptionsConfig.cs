using Microsoft.AspNetCore.Identity;
using Webion.AspNetCore;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Config;

public sealed class OptionsConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<ClickUpSettings>()
            .BindConfiguration(ClickUpSettings.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public void Use(WebApplication app)
    {
        throw new NotImplementedException();
    }
}