namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Items.Add;

public sealed class AddInvoiceItemRequest
{
    public decimal TotalUnits { get; init; }
    public string? Description { get; init; }
    
    public decimal NetPrice { get; init; }
    public decimal VatPercentage { get; init; }
}