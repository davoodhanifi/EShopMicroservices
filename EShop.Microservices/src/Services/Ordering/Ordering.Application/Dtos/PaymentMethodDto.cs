namespace Ordering.Application.Dtos;

public record PaymentMethodDto(string CardName, string CardNumber, string Expiration, string Cvv, int PaymentMethodType);