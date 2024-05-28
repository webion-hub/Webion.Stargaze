using Refit;

namespace Webion.Stargaze.Api.IntegrationTests.Api.Client;

public sealed class StargazeApi(HttpClient client)
{
    private readonly HttpClient _client = client;

    public IStargazeTimePackagesApi TimePackages => RestService.For<IStargazeTimePackagesApi>(_client);
}