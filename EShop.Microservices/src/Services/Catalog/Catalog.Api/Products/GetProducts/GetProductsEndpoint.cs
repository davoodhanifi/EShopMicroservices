using System.Xml.Linq;

namespace Catalog.Api.Products.CreateProduct;

public class GetProductsEndpoint : ICarterModule
{
    public record GetProductsRequest(Guid Id);
    public record GetProductsResponse(IEnumerable<Product> Products);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender mediator) =>
        {
            var result = await mediator.Send(new GetProductsQuery());
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
