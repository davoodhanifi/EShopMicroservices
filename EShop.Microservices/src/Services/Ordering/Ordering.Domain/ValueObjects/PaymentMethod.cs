namespace Ordering.Domain.ValueObjects;

public record PaymentMethod
{
    public string? CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public int PaymentMethodType { get; } = default!;
}
