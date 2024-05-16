using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.Dtos;

public abstract class PaginatedRequest
{
    public int Page { get; init; }

    [Range(1, 100)]
    public required int PageSize { get; init; }
}