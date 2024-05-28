using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.IntegrationTests.Seeders;

internal interface ISeeder
{
    void Seed(StargazeDbContext db);
}

internal static class SeederExtensions
{
    internal static void Seed<T>(this StargazeDbContext db)
        where T : ISeeder, new()
    {
        new T().Seed(db);
    }
}