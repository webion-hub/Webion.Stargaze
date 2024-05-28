using Webion.Stargaze.Api.IntegrationTests.Seeders;

namespace Webion.Stargaze.Api.IntegrationTests.Api.Controllers.v1.TimePackages;

public sealed class GetAllTimePackagesTest : IAssemblyFixture<StargazeWebApplicationFactory>
{
    private readonly StargazeWebApplicationFactory _factory;

    public GetAllTimePackagesTest(StargazeWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_all_time_packages_should_return_200()
    {
        // Arrange
        var client = _factory.Api.TimePackages;

        var kAuthTime = ProjectsSeeder.KAuth.Tasks
            .SelectMany(x => x.TimeEntries
                .Select(x => x.Duration.TotalHours))
            .Sum();
        var kTraceTime = ProjectsSeeder.KTrace.Tasks
            .SelectMany(x => x.TimeEntries
                .Select(x => x.Duration.TotalHours))
            .Sum();
        var kTrendTime = ProjectsSeeder.KTrend.Tasks
            .SelectMany(x => x.TimeEntries
                .Select(x => x.Duration.TotalHours))
            .Sum();

        var totalTime = kAuthTime + kTraceTime + kTrendTime;

        // Act
        var response = await client.GetAllTimePackagesTestAsync();

        // Assert
        Assert.NotEmpty(response.TimePackages);

        Assert.Equal(totalTime, response.TotalTime, 5);
        Assert.Equal(0, response.RemainingBillableTime);
    }
}