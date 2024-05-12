namespace Webion.Stargaze.Auth.Services.Clients;

public interface IClientsManager
{
    /// <summary>
    /// Verifies the client's credentials against the specified identifier and base64-encoded secret.
    /// </summary>
    /// <param name="id">The identifier of the client.</param>
    /// <param name="base64Secret">The base64-encoded secret of the client.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a boolean value indicating whether the verification was successful or not.
    /// </returns>
    Task<bool> VerifyAsync(Guid id, string base64Secret, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies the client's credentials against the specified identifier and secret.
    /// </summary>
    /// <param name="id">The identifier of the client.</param>
    /// <param name="secret">The binary secret key of the client.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a boolean value indicating whether the verification was successful or not.
    /// </returns>
    Task<bool> VerifyAsync(Guid id, byte[] secret, CancellationToken cancellationToken);
}