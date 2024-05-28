using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.GetAll;

public sealed class GetAllPaymentsRequest : PaginatedRequest
{
    public Guid? InvoiceId { get; init; }
    public Guid? BankAccountId { get; init; }
    public Guid? CategoryId { get; init; }
}