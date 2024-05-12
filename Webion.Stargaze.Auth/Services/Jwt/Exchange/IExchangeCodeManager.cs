namespace Webion.Stargaze.Auth.Services.Jwt.Exchange;

public interface IExchangeCodeManager
{
    /// <summary>
    /// Generates a unique authorization code for token exchange based on the given token pair.
    /// </summary>
    /// <param name="tokens">The token pair containing the access token and refresh token.</param>
    /// <returns>A unique authorization code.</returns>
    ValueTask<Guid> GetCodeAsync(TokenPair tokens);

    /// <summary>
    /// Exchanges a unique authorization code for a token pair (access token and refresh token).
    /// </summary>
    /// <param name="code">The unique authorization code obtained through token exchange.</param>
    /// <returns>The token pair (access token and refresh token) obtained through the code exchange.</returns>
    ValueTask<TokenPair?> ExchangeCodeAsync(Guid code);
}