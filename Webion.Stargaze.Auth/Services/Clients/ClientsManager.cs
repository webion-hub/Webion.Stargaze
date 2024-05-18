using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webion.Stargaze.Auth.Core;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Connect;

namespace Webion.Stargaze.Auth.Services.Clients;

public sealed class ClientsManager : IClientsManager
{
    private readonly StargazeDbContext _db;
    private readonly ILogger<ClientsManager> _logger;

    public ClientsManager(StargazeDbContext db, ILogger<ClientsManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<CreateClientResult?> CreateAsync(string name, CancellationToken cancellationToken)
    {
        var clientName = name.Trim();

        var isDuplicate = await _db.Clients
            .Where(x => x.Name.ToUpper() == clientName.ToUpper())
            .AnyAsync(cancellationToken);

        if (isDuplicate)
        {
            _logger.LogWarning("A client with name {Name} already exists", clientName);
            return null;
        }
        
        var secretBytes = RandomNumberGenerator.GetBytes(64);
        var base64Secret = Convert.ToBase64String(secretBytes);
        var hashedSecret = SecretsHasher.Hash(secretBytes);
        
        var client = new ClientDbo
        {
            Name = clientName,
            Secret = hashedSecret,
        };

        _db.Clients.Add(client);
        await _db.SaveChangesAsync(cancellationToken);
        return new CreateClientResult(
            Client: client,
            PlainTextSecret: base64Secret
        );
    }

    public async Task<bool> VerifyAsync(Guid id, string base64Secret, CancellationToken cancellationToken)
    {
        var secret = Convert.FromBase64String(base64Secret);
        return await VerifyAsync(id, secret, cancellationToken);
    }

    public async Task<bool> VerifyAsync(Guid id, byte[] secret, CancellationToken cancellationToken)
    {
        var client = await _db.Clients
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (client is null)
        {
            _logger.LogWarning("Could not find client with id {Id}", id);
            return false;
        }
        
        if (!SecretsHasher.Verify(secret, client.Secret))
        {
            _logger.LogWarning("Secrets do not match for client {ClientId}", client.Id);
            return false;
        }

        return true;
    }
}