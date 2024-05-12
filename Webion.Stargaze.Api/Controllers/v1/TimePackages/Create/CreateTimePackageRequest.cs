using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.Create;

public sealed class CreateTimePackageRequest
{
    [Required]
    public Guid CompanyId { get; init; }
    
    [Required]
    public int Hours { get; init; }
    
    public string? Name { get; init; }
    public IEnumerable<Guid> AppliesToProjects { get; init; } = [];
}