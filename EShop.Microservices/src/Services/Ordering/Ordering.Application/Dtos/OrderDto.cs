using Ordering.Domain.Enums;

namespace Ordering.Application.Dtos;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    PaymentMethodDto PaymentMethod,
    OrderStatus Status,
    List<OrderItemDto> OrderItems);