using Microsoft.AspNetCore.Authentication.OAuth;

namespace Webion.AspNetCore.Authentication.ClickUp;

public sealed class ClickUpOptions : OAuthOptions
{
    public ClickUpOptions()
    {
        CallbackPath = new PathString("/signin-clickup");
        AuthorizationEndpoint = ClickUpDefaults.AuthorizationEndpoint;
        TokenEndpoint = ClickUpDefaults.TokenEndpoint;
        UserInformationEndpoint = ClickUpDefaults.UserInformationEndpoint;
    }
}