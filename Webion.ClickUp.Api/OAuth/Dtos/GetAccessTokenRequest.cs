using Refit;

namespace Webion.ClickUp.Api.OAuth.Dtos;

public sealed class GetAccessTokenRequest
{
    [AliasAs("client_id")]
    public required string ClientId { get; init; }
    
    [AliasAs("client_secret")]
    public required string ClientSecret { get; init; }
    
    [AliasAs("code")]
    public required string Code { get; init; }
}