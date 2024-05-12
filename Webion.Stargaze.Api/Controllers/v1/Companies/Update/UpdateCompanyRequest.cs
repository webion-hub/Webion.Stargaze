using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Companies.Update;

public sealed class UpdateCompanyRequest
{
    [Required]
    public string Name { get; init; } = null!;
}