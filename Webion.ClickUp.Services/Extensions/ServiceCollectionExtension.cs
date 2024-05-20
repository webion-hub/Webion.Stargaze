using Microsoft.Extensions.DependencyInjection;
using Webion.Stargaze.Services.Link;
using Webion.Stargaze.Services.Sync;

namespace Webion.Stargaze.Services.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ClickUpLinkerService>();
        services.AddTransient<SyncProjectTasksService>();
        return services;
    }
}