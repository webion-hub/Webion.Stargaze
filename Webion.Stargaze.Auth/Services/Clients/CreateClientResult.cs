using Webion.Stargaze.Pgsql.Entities.Connect;

namespace Webion.Stargaze.Auth.Services.Clients;

public sealed record CreateClientResult(ClientDbo Client, string PlainTextSecret);

