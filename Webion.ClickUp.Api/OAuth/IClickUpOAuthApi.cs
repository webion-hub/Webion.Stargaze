using Refit;
using Webion.ClickUp.Api.OAuth.Dtos;

namespace Webion.ClickUp.Api.OAuth;

public interface IClickUpOAuthApi
{
    [Post("/v2/oauth/token")]
    Task<GetAccessTokenResponse> GetAccessTokenAsync([Query] GetAccessTokenRequest request);

    [Get("/v2/user")]
    Task<GetAuthorizedUserResponse> GetUserAsync();
    
    [Get("/v2/user")]
    Task<GetAuthorizedUserResponse> GetUserAsync([Header("Authorization")] string accessToken);
}