using Webion.AspNetCore;
using Webion.Stargaze.Api.Options;
using Webion.Stargaze.Auth.Settings;

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

        builder.Services
            .AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public void Use(WebApplication app)
    {
    }
}