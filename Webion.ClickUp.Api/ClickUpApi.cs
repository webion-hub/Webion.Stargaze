using System.Text.Json;
using Webion.ClickUp.Api.Team;
using Refit;
using Webion.ClickUp.Api.Converters;
using Webion.ClickUp.Api.OAuth;
using Webion.ClickUp.Api.Users;

namespace Webion.ClickUp.Api;

internal sealed class ClickUpApi : IClickUpApi
{
    private readonly HttpClient _client;

    public IClickUpTeamApi Teams => RestService.For<IClickUpTeamApi>(_client, Settings);
    public IClickUpOAuthApi OAuth => RestService.For<IClickUpOAuthApi>(_client, Settings);
    public IClickUpUsersApi Users => RestService.For<IClickUpUsersApi>(_client, Settings);
    
    public ClickUpApi (HttpClient client)
    {
        _client = client;
    }
    
    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters = { new ClickUpIdConverter() }
        }),
    };

}