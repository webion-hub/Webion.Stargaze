using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class InvoiceDto
{
    public required Guid Id { get; init; }
    public required Guid? IssuedById { get; init; }
    public required Guid? IssuedToId { get; init; }

    public required decimal NetPrice { get; init; }
    public required decimal TaxedPrice { get; init; }
    public required decimal VatPercentage { get; init; }

    public required bool Paid { get; init; }

    public required MovementType Type { get; init; }
    public required DateTimeOffset EmittedAt { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }
}