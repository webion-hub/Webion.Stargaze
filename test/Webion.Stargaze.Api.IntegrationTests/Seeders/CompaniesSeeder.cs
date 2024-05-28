using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Core;

namespace Webion.Stargaze.Api.IntegrationTests.Seeders;

public class CompaniesSeeder : ISeeder
{
    public static CompanyDbo Kaire { get; }

    static CompaniesSeeder()
    {
        Kaire = new CompanyDbo
        {
            Name = "Kaire Automation",
        };
    }

    public void Seed(StargazeDbContext db)
    {
        db.Companies.Add(Kaire);
        db.SaveChanges();
    }
}