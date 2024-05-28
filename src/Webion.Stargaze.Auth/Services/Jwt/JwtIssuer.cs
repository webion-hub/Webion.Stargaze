using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;
using Webion.Stargaze.Auth.Settings;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Services.Jwt;


public sealed class JwtIssuer : IJwtIssuer
{
    private readonly IUserClaimsPrincipalFactory<UserDbo> _principalFactory;
    private readonly JwtSecurityTokenHandler _jwtHandler = new();
    private readonly JwtSettings _options;
    private readonly TimeProvider _timeProvider;
    private readonly IRefreshTokenManager _refreshTokenManager;
    private readonly ILogger<JwtIssuer> _logger;

    public JwtIssuer(IUserClaimsPrincipalFactory<UserDbo> principalFactory, IOptions<JwtSettings> options, TimeProvider timeProvider, ILogger<JwtIssuer> logger, IRefreshTokenManager refreshTokenManager)
    {
        _principalFactory = principalFactory;
        _timeProvider = timeProvider;
        _logger = logger;
        _refreshTokenManager = refreshTokenManager;
        _options = options.Value;
    }

    public async Task<TokenPair> IssuePairAsync(UserDbo user, ClientDbo client, CancellationToken cancellationToken)
    {
        var userPrincipal = await _principalFactory.CreateAsync(user);
        var claims = new Claim[]
        {
            new(JwtClaims.Sub, user.Id.ToString()),
            new(JwtClaims.Name, user.UserName!),
        };

        var roles = userPrincipal.Claims
            .Where(c => c.Type is ClaimTypes.Role)
            .Select(c => new Claim(JwtClaims.Roles, c.Value));

        var scopes = userPrincipal.Claims
            .Where(c => c.Type is Claims.Scopes)
            .Select(c => new Claim(JwtClaims.Scopes, c.Value));

        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var jwtDescriptor = new JwtSecurityToken(
            claims: claims
                .Concat(roles)
                .Concat(scopes),

            notBefore: utcNow,
            expires: utcNow + _options.AccessTokenDuration,
            issuer: _options.Issuer,
            audience: "https://stargaze.webion.it",
            signingCredentials: new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
                algorithm: SecurityAlgorithms.HmacSha512Signature
            )
        );

        var accessToken = _jwtHandler.WriteToken(jwtDescriptor);
        var refreshToken = await _refreshTokenManager.GenerateAsync(user, client, cancellationToken);

        _logger.LogInformation("Issued token pair for user {UserId}", user.Id);
        return new TokenPair(accessToken, refreshToken);
    }
}