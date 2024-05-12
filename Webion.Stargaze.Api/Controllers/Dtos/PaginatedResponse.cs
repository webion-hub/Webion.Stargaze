using Webion.Extensions.Linq.Abstractions;

namespace Webion.Stargaze.Api.Controllers.Dtos;

public abstract class PaginatedResponse<T, TImpl>
    where TImpl : PaginatedResponse<T, TImpl>, new()
{
    public IEnumerable<T> Results { get; init; } = [];
    public int TotalPages { get; init; }
    public int TotalResults { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }

    
    public static TImpl From<Z>(PaginatedResult<Z> page, Func<Z, T> map)
    {
        return new TImpl
        {
            Results = page.Results.Select(map),
            TotalPages = page.TotalPages,
            TotalResults = page.TotalResults,
            HasNextPage = page.HasNextPage,
            HasPreviousPage = page.HasPreviousPage,
        };
    }
    
    public static TImpl From(PaginatedResult<T> page)
    {
        return new TImpl
        {
            Results = page.Results,
            TotalPages = page.TotalPages,
            TotalResults = page.TotalResults,
            HasNextPage = page.HasNextPage,
            HasPreviousPage = page.HasPreviousPage,
        };
    }
}