using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.IntegrationTests.Seeders;

public class TimePackagesSeeder : ISeeder
{
    public static TimePackageDbo KaireSenior { get; }
    public static TimePackageDbo KaireJunior { get; }

    static TimePackagesSeeder()
    {
        KaireSenior = new TimePackageDbo
        {
            Hours = 100,
            Name = "Ore senior",
            Company = CompaniesSeeder.Kaire,
            AppliesTo = [ProjectsSeeder.KTrace, ProjectsSeeder.KTrend, ProjectsSeeder.KAuth],
            Rates = [
                new TimePackageRateDbo { Rate = 40, User = UsersSeeder.Stefano },
                new TimePackageRateDbo { Rate = 40, User = UsersSeeder.Alessandro },
                new TimePackageRateDbo { Rate = 40, User = UsersSeeder.Matteo },
                new TimePackageRateDbo { Rate = 40, User = UsersSeeder.Davide },
            ]
        };

        KaireJunior = new TimePackageDbo
        {
            Hours = 250,
            Name = "Ore junior",
            Company = CompaniesSeeder.Kaire,
            AppliesTo = [ProjectsSeeder.KTrace, ProjectsSeeder.KTrend, ProjectsSeeder.KAuth],
            Rates = [
                new TimePackageRateDbo { Rate = 30, User = UsersSeeder.Jacopo },
                new TimePackageRateDbo { Rate = 30, User = UsersSeeder.Francesco },
                new TimePackageRateDbo { Rate = 30, User = UsersSeeder.Filippo },
                new TimePackageRateDbo { Rate = 30, User = UsersSeeder.Daniele },
                new TimePackageRateDbo { Rate = 30, User = UsersSeeder.Lorenzo },
            ]
        };
    }

    public void Seed(StargazeDbContext db)
    {
        db.TimePackages.AddRange(KaireSenior, KaireJunior);
        db.SaveChanges();
    }
}