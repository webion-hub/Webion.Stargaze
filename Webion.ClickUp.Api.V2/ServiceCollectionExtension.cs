using Microsoft.Extensions.DependencyInjection;

namespace Webion.ClickUp.Api.V2;

public static class ServiceCollectionExtension
{
    public static IHttpClientBuilder AddClickUpApi(this IServiceCollection services)
    {
        return services
            .AddHttpClient<IClickUpApi, ClickUpApi>()
            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri("https://api.clickup.com/api");
            });
    }
}