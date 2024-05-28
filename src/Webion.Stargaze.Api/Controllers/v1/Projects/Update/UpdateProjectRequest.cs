using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Update;

public sealed class UpdateProjectRequest
{
    [Required]
    public Guid CompanyId { get; init; }

    [Required]
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}