namespace Webion.Stargaze.Cli.Core;

public interface ICliApplicationLifetime
{
    CancellationToken CancellationToken { get; }
}