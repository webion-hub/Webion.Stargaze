using Microsoft.Extensions.DependencyInjection;
using Webion.Stargaze.Services.Link;

namespace Webion.Stargaze.Services.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ClickUpLinkerService>();
        return services;
    }
}