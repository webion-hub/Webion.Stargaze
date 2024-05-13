using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Services.Jwt;

public interface IJwtIssuer
{
    /// <summary>
    /// Generates a token pair (access token and refresh token) for the given user and client.
    /// </summary>
    /// <param name="user">The user for whom the tokens are generated.</param>
    /// <param name="client">The client requesting the tokens.</param>
    /// <returns>The generated token pair.</returns>
    Task<TokenPair> IssuePairAsync(UserDbo user);
}