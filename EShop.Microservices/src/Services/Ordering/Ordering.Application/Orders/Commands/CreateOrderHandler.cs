namespace Ordering.Application.Orders.Commands;

public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    private readonly IApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName,
                                         orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine,
                                         orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State,
                                         orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName,
                                        orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine,
                                        orderDto.BillingAddress.Country, orderDto.BillingAddress.State,
                                        orderDto.BillingAddress.ZipCode);
        var paymentMethod = PaymentMethod.Of(orderDto.PaymentMethod.CardName, orderDto.PaymentMethod.CardNumber,
                                             orderDto.PaymentMethod.Expiration, orderDto.PaymentMethod.Cvv, 
                                             orderDto.PaymentMethod.PaymentMethodType);

        var order = Order.Create(OrderId.Of(Guid.NewGuid()), 
                            CustomerId.Of(orderDto.CustomerId), 
                            OrderName.Of(orderDto.OrderName),
                            shippingAddress, 
                            billingAddress, 
                            paymentMethod);

        foreach (var orderItem in orderDto.OrderItems)
        {
            order.AddOrderItem(ProductId.Of(orderItem.ProductId), orderItem.Quantity, orderItem.Price);
        }

        return order;
    }
}
