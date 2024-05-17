using Webion.Stargaze.Core.Enums;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.Update;

public sealed class UpdatePaymentRequest
{
    public Guid? BankAccountId { get; init; }
    public Guid? CategoryId { get; init; }

    public string? From { get; init; }
    public string? To { get; init; }
    public string? Description { get; init; }

    public decimal Amount { get; init; }
    public MovementType Type { get; init; }
    public PaymentStatus Status { get; init; }

    public DateTimeOffset PaidAt { get; init; }
}