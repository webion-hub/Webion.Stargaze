using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Api.Controllers.Dtos;

public sealed class PaymentDto
{
    public required Guid Id { get; init; }
    public required Guid? InvoiceId { get; init; }
    public required Guid? BankAccountId { get; init; }
    public required Guid? CategoryId { get; init; }


    public required string? From { get; init; }
    public required string? To { get; init; }
    public required string? Description { get; init; }

    public required decimal Amount { get; init; }
    public required MovementType Type { get; init; }
    public required PaymentStatus Status { get; init; }
}