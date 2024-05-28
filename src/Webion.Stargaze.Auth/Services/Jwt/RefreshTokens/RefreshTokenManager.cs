using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Auth.Settings;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Connect;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Auth.Services.Jwt.RefreshTokens;

internal sealed class RefreshTokenManager : IRefreshTokenManager
{
    private readonly StargazeDbContext _db;
    private readonly JwtSettings _options;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<RefreshTokenManager> _logger;

    public RefreshTokenManager(StargazeDbContext db, TimeProvider timeProvider, IOptions<JwtSettings> options, ILogger<RefreshTokenManager> logger)
    {
        _db = db;
        _timeProvider = timeProvider;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<RefreshTokenDbo?> RetrieveAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var isValid = RefreshTokenSerializer.TryDeserialize(refreshToken, out var token);
        if (!isValid)
            return null;
        
        return await _db.RefreshTokens
            .Where(x => x.Id == token.Id)
            .Where(r => r.ExpiresAt >= _timeProvider.GetUtcNow())
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    
    public async Task<string> GenerateAsync(UserDbo user, ClientDbo client, CancellationToken cancellationToken)
    {
        var expiresAt = _timeProvider.GetUtcNow() + _options.RefreshTokenDuration;
        var secret = RandomNumberGenerator.GetBytes(RefreshToken.SecretSize);
        var token = new RefreshTokenDbo
        {
            UserId = user.Id,
            ClientId = client.Id,
            Secret = SecretsHasher.Hash(secret),
            ExpiresAt = expiresAt,
        };

        _db.RefreshTokens.Add(token);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Generated refresh token for user {UserId}, {@Token}", user.Id, token);
        return RefreshTokenSerializer.Serialize(token.Id, secret);
    }
}