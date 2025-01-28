namespace Ordering.Domain.ValueObjects;

public record PaymentMethod
{
    public string? CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string CVV { get; } = default!;
    public int PaymentMethodType { get; } = default!;

    protected PaymentMethod()
    {
    }

    private PaymentMethod(string cardName, string cardNumber, string expiration, string cvv, int paymentMethodType)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethodType = paymentMethodType;
    }

    public static PaymentMethod Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethodType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        return new PaymentMethod(cardName, cardNumber, expiration, cvv, paymentMethodType);
    }
}
