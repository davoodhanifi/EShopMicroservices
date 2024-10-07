using System.Xml.Linq;

namespace Catalog.Api.Products.CreateProduct;

public class GetProductEndpoint : ICarterModule
{
    public record GetProductRequest(Guid Id);
    public record GetProductResponse(Product Product);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetProductQuery(id));
            var response = result.Adapt<GetProductResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProduct")
        .Produces<GetProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product")
        .WithDescription("Get Product");
    }
}
