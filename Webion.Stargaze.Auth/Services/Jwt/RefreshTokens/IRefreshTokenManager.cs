using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;

public interface IRefreshTokenManager
{
    /// <summary>
    /// Retrieves a refresh token from the database based on the provided refresh token string.
    /// </summary>
    /// <remarks>
    /// If the refresh token is expired, no result will be returned.
    /// </remarks>
    /// <param name="refreshToken">The refresh token string to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The refresh token object retrieved from the database, or null if no matching refresh token is found.
    /// </returns>
    Task<RefreshTokenDbo?> RetrieveAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Generates a new refresh token for the provided user and client.
    /// </summary>
    /// <param name="user">The user for which to generate the refresh token.</param>
    /// <param name="client">The client for which to generate the refresh token.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The generated refresh token string.
    /// </returns>
    Task<string> GenerateAsync(UserDbo user, ClientDbo client, CancellationToken cancellationToken);
}