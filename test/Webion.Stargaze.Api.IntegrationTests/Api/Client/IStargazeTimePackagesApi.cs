using Refit;
using Webion.Stargaze.Api.Controllers.v1.TimePackages.GetAll;

namespace Webion.Stargaze.Api.IntegrationTests.Api.Client;

public interface IStargazeTimePackagesApi
{
    [Get("/v1.0/time/packages")]
    Task<GetAllTimePackagesResponse> GetAllTimePackagesTestAsync();
}