using Webion.Stargaze.Pgsql.Entities.Identity;
using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Api.Controllers.Entities;

public class EntryDetailEntity(
    ProjectDbo project,
    UserDbo user,
    TimeSpan trackedTime
)
{
    public ProjectDbo Project { get; init; } = project;
    public UserDbo User { get; init; } = user;
    public TimeSpan TrackedTime { get; set; } = trackedTime;
}