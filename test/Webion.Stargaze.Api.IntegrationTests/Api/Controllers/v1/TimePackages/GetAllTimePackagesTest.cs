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

        // Act
        var response = await client.GetAllTimePackagesTestAsync();

        // Assert
        Assert.NotEmpty(response.Packages);
        Assert.NotEmpty(response.AppliesTo);

        // TODO
        Assert.NotEqual(int.MinValue, response.TotalTime);
        Assert.NotEqual(int.MinValue, response.RemainingBillableTime);
    }
}