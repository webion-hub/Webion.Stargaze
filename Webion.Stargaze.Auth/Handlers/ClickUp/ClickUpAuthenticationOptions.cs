using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Webion.Stargaze.Auth.Handlers.ClickUp;

public sealed class ClickUpAuthenticationOptions : OAuthOptions
{
    public const string DefaultScheme = "ClickUp";

    public ClickUpAuthenticationOptions()
    {
        CallbackPath = new PathString("/signin-clickup");
        AuthorizationEndpoint = "https://app.clickup.com/api";
        TokenEndpoint = "https://app.clickup.com/api/v2/oauth/token";
        UserInformationEndpoint = "https://app.clickup.com/api/v2/user";
    }
}