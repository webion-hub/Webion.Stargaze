using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;

namespace Webion.Stargaze.Api.IntegrationTests.Seeders;

public class UsersSeeder : ISeeder
{
    public static UserDbo Stefano { get; }
    public static UserDbo Alessandro { get; }
    public static UserDbo Matteo { get; }
    public static UserDbo Davide { get; }
    public static UserDbo Jacopo { get; }
    public static UserDbo Francesco { get; }
    public static UserDbo Filippo { get; }
    public static UserDbo Daniele { get; }
    public static UserDbo Lorenzo { get; }

    static UsersSeeder()
    {
        Stefano = new UserDbo { UserName = "Stefano" };
        Alessandro = new UserDbo { UserName = "Alessandro" };
        Matteo = new UserDbo { UserName = "Matteo" };
        Davide = new UserDbo { UserName = "Davide" };
        Jacopo = new UserDbo { UserName = "Jacopo" };
        Francesco = new UserDbo { UserName = "Francesco" };
        Filippo = new UserDbo { UserName = "Filippo" };
        Daniele = new UserDbo { UserName = "Daniele" };
        Lorenzo = new UserDbo { UserName = "Lorenzo" };
    }

    public void Seed(StargazeDbContext db)
    {
        db.Users.AddRange(
            Stefano,
            Alessandro,
            Matteo,
            Davide,
            Jacopo,
            Francesco,
            Filippo,
            Daniele,
            Lorenzo
        );

        db.SaveChanges();
    }
}