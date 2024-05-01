using System.Text.Json;
using Webion.ClickUp.Api.Team;
using Refit;

namespace Webion.ClickUp.Api;

public sealed class ClickUpApi
{
    private readonly HttpClient _client;

    public IClickUpTeamApi Teams => RestService.For<IClickUpTeamApi>(_client, Settings); 

    public ClickUpApi (HttpClient client)
    {
        _client = client;
    }
    
    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        }),
    };

}