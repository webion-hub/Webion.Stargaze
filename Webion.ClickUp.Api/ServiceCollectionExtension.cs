using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Webion.ClickUp.Api;

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