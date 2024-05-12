using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Create;

public sealed class CreateCompanyRequest
{
    [Required]
    public string Name { get; init; } = null!;
}