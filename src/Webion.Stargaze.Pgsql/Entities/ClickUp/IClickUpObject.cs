using Webion.Stargaze.Pgsql.Entities.Projects;

namespace Webion.Stargaze.Pgsql.Entities.ClickUp;

public interface IClickUpObject
{
    public string Id { get; }
    public List<ProjectDbo> Projects { get; }
}