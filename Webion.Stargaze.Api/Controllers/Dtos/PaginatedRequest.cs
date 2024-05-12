namespace Webion.Stargaze.Api.Controllers.Dtos;

public abstract class PaginatedRequest
{
    public int Page { get; init; }
    public int PageSize { get; init; }
}