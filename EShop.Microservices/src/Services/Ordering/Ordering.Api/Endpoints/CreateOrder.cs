using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Api.Endpoints;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, IMediator sender) =>
        {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateOrderResponse>();
            return Results.Created($"/orders/{response.Id}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new order")
        .WithDescription("Creates a new order in the system. The order must include customer details, shipping and billing addresses, payment method, and at least one order item.");
    }
}