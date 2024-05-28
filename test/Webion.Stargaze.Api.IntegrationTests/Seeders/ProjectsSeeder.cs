using Bogus;
using Webion.Stargaze.Pgsql;
using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.Projects;
using Webion.Stargaze.Pgsql.Entities.TimeTracking;

namespace Webion.Stargaze.Api.IntegrationTests.Seeders;

public class ProjectsSeeder : ISeeder
{
    public static ProjectDbo KTrace { get; }
    public static ProjectDbo KPerformance { get; }
    public static ProjectDbo KAuth { get; }
    public static ProjectDbo KTrend { get; }

    static ProjectsSeeder()
    {
        var allUsers = new List<UserDbo> {
            UsersSeeder.Stefano,
            UsersSeeder.Alessandro,
            UsersSeeder.Matteo,
            UsersSeeder.Davide,
            UsersSeeder.Jacopo,
            UsersSeeder.Francesco,
            UsersSeeder.Filippo,
            UsersSeeder.Daniele,
            UsersSeeder.Lorenzo
        };

        var fakeTasks = new Faker<TaskDbo>()
            .RuleFor(x => x.Title, f => f.Lorem.Sentence())
            .RuleFor(x => x.Description, f => f.Lorem.Sentences())
            .RuleFor(x => x.Assignees, f =>
            [
                f.PickRandom(allUsers),
                f.PickRandom(allUsers)
            ])
            .RuleFor(x => x.TimeEntries, _ => new Faker<TimeEntryDbo>()
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(x => x.Billable, _ => true)
                .RuleFor(x => x.User, f => f.PickRandom(allUsers))
                .RuleFor(x => x.Start, f => f.Date.Past().ToUniversalTime())
                .RuleFor(x => x.Duration, f => f.Date.Timespan(TimeSpan.FromHours(3)))
                .RuleFor(x => x.End, (_, t) => t.Start + t.Duration)
                .Generate(3)
                .ToList()
            );

        KTrace = new ProjectDbo
        {
            Name = "KTrace",
            Company = CompaniesSeeder.Kaire,
            Tasks = fakeTasks.Generate(10)
        };

        KPerformance = new ProjectDbo
        {
            Name = "KPerformance",
            Company = CompaniesSeeder.Kaire,
            Tasks = fakeTasks.Generate(10)
        };

        KAuth = new ProjectDbo
        {
            Name = "KAuth",
            Company = CompaniesSeeder.Kaire,
            Tasks = fakeTasks.Generate(10)
        };

        KTrend = new ProjectDbo
        {
            Name = "KTrend",
            Company = CompaniesSeeder.Kaire,
            Tasks = fakeTasks.Generate(10)
        };
    }

    public void Seed(StargazeDbContext db)
    {
        db.Projects.AddRange(KTrace, KPerformance, KAuth, KTrend);
        db.SaveChanges();
    }
}