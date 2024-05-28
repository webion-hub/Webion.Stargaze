namespace Webion.Stargaze.Api.Controllers.v1.Companies.Get;

public sealed class GetCompanyResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}