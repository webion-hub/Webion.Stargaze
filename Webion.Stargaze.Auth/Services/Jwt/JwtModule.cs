using Microsoft.Extensions.DependencyInjection;
using Webion.Application.Extensions;
using Webion.Stargaze.Auth.Services.Jwt.Exchange;

namespace Webion.Stargaze.Auth.Services.Jwt;


public sealed class JwtModule : IModule
{
    public void Configure(IServiceCollection services)
    {
        services.AddTransient<IJwtIssuer, JwtIssuer>();
        services.AddTransient<IExchangeCodeManager, ExchangeCodeManager>();
    }
}