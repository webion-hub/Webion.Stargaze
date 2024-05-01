using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Webion.ClickUp.Api.Team;

namespace Webion.ClickUp.Api;

public static class ServiceCollectionExtension
{
    public static void AddClickUpApi(this IServiceCollection services, string apiKey)
    {
        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            }),
        };

        services
            .AddRefitClient<IClickUpTeamApi>(settings)
            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri("https://api.clickup.com/api");
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiKey);
            });

        services.AddTransient<ClickUpApi>();
    }
}