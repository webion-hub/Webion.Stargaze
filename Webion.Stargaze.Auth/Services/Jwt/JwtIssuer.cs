using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;
using Webion.Stargaze.Auth.Settings;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Services.Jwt;


public sealed class JwtIssuer : IJwtIssuer
{
    private readonly IUserClaimsPrincipalFactory<UserDbo> _principalFactory;
    private readonly JwtSecurityTokenHandler _jwtHandler = new();
    private readonly JwtSettings _options;
    private readonly StargazeDbContext _db;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<JwtIssuer> _logger;

    public JwtIssuer(IUserClaimsPrincipalFactory<UserDbo> principalFactory, IOptions<JwtSettings> options, StargazeDbContext db, TimeProvider timeProvider, ILogger<JwtIssuer> logger)
    {
        _principalFactory = principalFactory;
        _db = db;
        _timeProvider = timeProvider;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<TokenPair> IssuePairAsync(UserDbo user)
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

        var jwtDescriptor = new JwtSecurityToken(
            claims: claims
                .Concat(roles)
                .Concat(scopes),

            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow + _options.AccessTokenDuration,
            issuer: _options.Issuer,
            audience: "https://stargaze.webion.it",
            signingCredentials: new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
                algorithm: SecurityAlgorithms.HmacSha512Signature
            )
        );

        var accessToken = _jwtHandler.WriteToken(jwtDescriptor);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        _logger.LogInformation("Issued token pair for user {UserId}", user.Id);
        return new TokenPair(accessToken, refreshToken);
    }


    private async Task<string> GenerateRefreshTokenAsync(UserDbo user)
    {
        var expiresAt = _timeProvider.GetUtcNow() + _options.RefreshTokenDuration;
        var secret = RandomNumberGenerator.GetBytes(RefreshToken.SecretSize);
        var token = new RefreshTokenDbo
        {
            UserId = user.Id,
            Secret = SecretsHasher.Hash(secret),
            ExpiresAt = expiresAt,
        };

        _db.RefreshTokens.Add(token);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Generated refresh token for user {UserId}, {@Token}", user.Id, token);
        return RefreshTokenSerializer.Serialize(token.Id, secret);
    }
}