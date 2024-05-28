using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Webion.AspNetCore;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.Stargaze.Auth.Extensions;
using Webion.Stargaze.Auth.Settings;

namespace Webion.Stargaze.Api.Config;

public sealed class AuthNConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.AddStargazeIdentity();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.Events.OnRedirectToAccessDenied = HandleRedirect(401);
            options.Events.OnRedirectToLogin = HandleRedirect(401);
            options.Events.OnRedirectToLogout = HandleRedirect(204);

            static Func<RedirectContext<CookieAuthenticationOptions>, Task> HandleRedirect(int status)
            {
                return (context) =>
                {
                    context.HttpContext.Response.StatusCode = status;
                    return Task.CompletedTask;
                };
            }
        });

        var jwtSettings = builder.Configuration
            .GetSection(JwtSettings.Section)
            .Get<JwtSettings>()!;

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Audience = "https://stargaze.webion.it";
                options.TokenValidationParameters.ValidIssuer = jwtSettings.Issuer;
                options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)
                );
            })
            .AddClickUp(options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClientId = builder.Configuration["ClickUp:ClientId"]!;
                options.ClientSecret = builder.Configuration["ClickUp:ClientSecret"]!;
            })
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClientId = builder.Configuration["Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
            });
    }

    public void Use(WebApplication app)
    {
        app.UseAuthentication();
    }
}