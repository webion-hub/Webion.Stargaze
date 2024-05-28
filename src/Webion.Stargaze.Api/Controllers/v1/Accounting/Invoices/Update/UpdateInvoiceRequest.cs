using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.Update;

public sealed class UpdateInvoiceRequest
{
    public Guid? IssuedById { get; init; }
    public Guid? IssuedToId { get; init; }
    
    public decimal NetPrice { get; init; }
    public decimal VatPercentage { get; init; }
    
    public bool Paid { get; init; }
    
    public MovementType Type { get; init; }
    public DateTimeOffset EmittedAt { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}