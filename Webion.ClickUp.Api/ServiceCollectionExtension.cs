using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Webion.ClickUp.Api;

public static class ServiceCollectionExtension
{
    public static void AddClickUpApi(this IServiceCollection services, string apiKey)
    {
        services
            .AddHttpClient<ClickUpApi>()
            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri("https://api.clickup.com/api");
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiKey);
            });
    }
}