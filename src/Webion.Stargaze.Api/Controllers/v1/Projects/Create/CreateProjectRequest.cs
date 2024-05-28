using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Create;

public sealed class CreateProjectRequest
{
    [Required]
    public Guid CompanyId { get; init; }
    
    [Required]
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
}