namespace Ordering.Application.Orders.EventHandlers;

public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
{
    private readonly ILogger<OrderUpdatedEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event Handled {DomainEvent}",
            notification.GetType().Name);

        await Task.CompletedTask;
    }
}