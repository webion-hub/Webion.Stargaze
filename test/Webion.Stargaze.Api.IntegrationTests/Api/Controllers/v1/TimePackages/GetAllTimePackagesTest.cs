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

        var seniorUsersIds = TimePackagesSeeder.KaireSenior.Rates
            .Select(x => x.UserId);
        var juniorUsersIds = TimePackagesSeeder.KaireJunior.Rates
            .Select(x => x.UserId);

        var timeEntries = ProjectsSeeder.KTrend.Tasks
                .SelectMany(x => x.TimeEntries)
                .ToList();
        timeEntries.AddRange(ProjectsSeeder.KAuth.Tasks
            .SelectMany(x => x.TimeEntries));
        timeEntries.AddRange(ProjectsSeeder.KTrace.Tasks
            .SelectMany(x => x.TimeEntries));

        var seniorHours = timeEntries
            .Where(x => seniorUsersIds.Contains(x.UserId))
            .Select(x => x.Duration.TotalHours)
            .Sum();

        var juniorHours = timeEntries
            .Where(x => juniorUsersIds.Contains(x.UserId))
            .Select(x => x.Duration.TotalHours)
            .Sum();

        // Act
        var response = await client.GetAllTimePackagesTestAsync();

        var seniorPackage = response.TimePackages
            .FirstOrDefault(x => x.Id == TimePackagesSeeder.KaireSenior.Id);
        var juniorPackage = response.TimePackages
            .FirstOrDefault(x => x.Id == TimePackagesSeeder.KaireJunior.Id);

        // Assert
        Assert.NotEmpty(response.TimePackages);

        Assert.Equal(seniorHours + juniorHours, response.TotalTime, 5);
        Assert.Equal(0, response.RemainingBillableTime, 5);

        Assert.Equal(seniorHours, seniorPackage!.TrackedHours, 5);
        Assert.Equal(juniorHours, juniorPackage!.TrackedHours, 5);
    }
}