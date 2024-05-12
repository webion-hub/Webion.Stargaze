using Microsoft.Extensions.DependencyInjection;
using Webion.Application.Extensions;

namespace Webion.Stargaze.Auth.Services.Clients;

public sealed class ClientsModule : IModule
{
    public void Configure(IServiceCollection services)
    {
        services.AddTransient<IClientsManager, ClientsManager>();
    }
}