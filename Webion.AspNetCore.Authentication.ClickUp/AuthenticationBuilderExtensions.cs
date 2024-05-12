using Microsoft.AspNetCore.Authentication;

namespace Webion.AspNetCore.Authentication.ClickUp;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddClickUp(this AuthenticationBuilder builder,
        Action<ClickUpOptions> configureOptions
    )
    {
        return builder.AddOAuth<ClickUpOptions, ClickUpHandler>(
            authenticationScheme: ClickUpDefaults.AuthenticationScheme,
            displayName: ClickUpDefaults.DisplayName,
            configureOptions: configureOptions
        );
    }
}