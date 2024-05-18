using Webion.Stargaze.Pgsql.Entities.Connect;

namespace Webion.Stargaze.Auth.Services.Clients;

public abstract record CreateClientResult
{
    public sealed record Duplicate : CreateClientResult;
    public sealed record Created(ClientDbo Client, string PlainTextSecret) : CreateClientResult;
};