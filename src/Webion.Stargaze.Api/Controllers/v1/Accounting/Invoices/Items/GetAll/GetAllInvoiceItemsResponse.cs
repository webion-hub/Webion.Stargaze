namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Items.GetAll;

public sealed class GetAllInvoiceItemsResponse
{
    public required Guid Id { get; init; }
    public required Guid InvoiceId { get; init; }

    public required decimal TotalUnits { get; init; }
    public required string? Description { get; init; }

    public required decimal NetPrice { get; init; }
    public required decimal TaxedPrice { get; init; }
    public required decimal VatPercentage { get; init; }
}