using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Services.Jwt;

public interface IJwtIssuer
{
    /// <summary>
    /// Issues a pair of tokens (access token and refresh token) for the specified user and client.
    /// </summary>
    /// <param name="user">The user for which to issue the tokens.</param>
    /// <param name="client">The client for which to issue the tokens.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
    /// The task result contains the issued token pair.</returns>
    Task<TokenPair> IssuePairAsync(UserDbo user, ClientDbo client, CancellationToken cancellationToken);
}