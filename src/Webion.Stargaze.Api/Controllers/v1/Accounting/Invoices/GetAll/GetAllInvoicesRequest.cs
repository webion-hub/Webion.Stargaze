using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.GetAll;

public sealed class GetAllInvoicesRequest : PaginatedRequest
{
    public Guid? IssuedById { get; init; }
    public Guid? IssuedToId { get; init; }
}