using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Webion.AspNetCore;
using Webion.AspNetCore.Authentication.ClickUp;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.Config;

public sealed class AuthNConfig : IWebApplicationConfiguration
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services
            .AddIdentity<UserDbo, RoleDbo>()
            .AddEntityFrameworkStores<StargazeDbContext>()
            .AddDefaultTokenProviders();
        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClientId = builder.Configuration["Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
            })
            .AddClickUp(options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClientId = builder.Configuration["ClickUp:ClientId"]!;
                options.ClientSecret = builder.Configuration["ClickUp:ClientSecret"]!;
            });
    }

    public void Use(WebApplication app)
    {
        app.UseAuthentication();
    }
}