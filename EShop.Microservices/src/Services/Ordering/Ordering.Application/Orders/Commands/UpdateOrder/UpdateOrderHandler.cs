using Ordering.Application.Exceptions;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    private readonly IApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await _dbContext.Orders.FindAsync([orderId], cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }

        UpdateOrder(order, command.Order);
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private void UpdateOrder(Order order, OrderDto orderDto)
    {
        var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var updatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var updatedPayment = PaymentMethod.Of(orderDto.PaymentMethod.CardName, orderDto.PaymentMethod.CardNumber, orderDto.PaymentMethod.Expiration, orderDto.PaymentMethod.Cvv, orderDto.PaymentMethod.PaymentMethodType);

        order.Update(OrderName.Of(orderDto.OrderName),
                    updatedShippingAddress,
                    updatedBillingAddress,
                    updatedPayment,
                    orderDto.Status);
    }
}
