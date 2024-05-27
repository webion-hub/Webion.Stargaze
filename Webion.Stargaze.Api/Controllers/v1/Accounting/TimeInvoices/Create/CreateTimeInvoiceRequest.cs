namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.TimeInvoices.Create;

public sealed class CreateTimeInvoiceRequest
{
    public Guid InvoiceId { get; init; }
    public Guid TimePackageId { get; init; }
    public TimeSpan InvoicedTime { get; init; }
}