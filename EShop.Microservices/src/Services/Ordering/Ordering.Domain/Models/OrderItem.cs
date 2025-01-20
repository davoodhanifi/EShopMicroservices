namespace Ordering.Domain.Models;

public class OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price) : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; } = orderId;
    public ProductId ProductId { get; private set; } = productId;
    public int Quantity { get; private set; } = quantity;
    public decimal Price { get; private set; } = price;
}
