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
        Stefano = new UserDbo();
        Alessandro = new UserDbo();
        Matteo = new UserDbo();
        Davide = new UserDbo();
        Jacopo = new UserDbo();
        Francesco = new UserDbo();
        Filippo = new UserDbo();
        Daniele = new UserDbo();
        Lorenzo = new UserDbo();
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