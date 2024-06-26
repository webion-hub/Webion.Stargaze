using Webion.Stargaze.Pgsql.Entities.Connect;

namespace Webion.Stargaze.Auth.Services.Clients;

public interface IClientsManager
{
    /// <summary>
    /// Retrieves a client entity with the specified client ID.
    /// </summary>
    /// <param name="clientId">The ID of the client to retrieve.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the retrieved client entity, or null if no client with the specified ID is found.</returns>
    Task<ClientDbo?> FindByIdAsync(Guid clientId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Creates a new client with the specified name and returns the created client entity.
    /// </summary>
    /// <param name="name">The name of the client.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the created client entity, or null if the client creation fails.
    /// </returns>
    Task<CreateClientResult> CreateAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies the client's credentials against the specified identifier and base64-encoded secret.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="base64Secret">The base64-encoded secret of the client.</param>
    /// <returns>A boolean value indicating whether the verification was successful or not.</returns>
    bool VerifySecret(ClientDbo client, string base64Secret);

    /// <summary>
    /// Verifies the client's credentials against the specified identifier and secret.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="secret">The binary secret key of the client.</param>
    /// <returns>A boolean value indicating whether the verification was successful or not.</returns>
    bool VerifySecret(ClientDbo client, byte[] secret);
}