
namespace Catalog.Api.Products.CreateProduct;

public class CreateProductEndpoint : ICarterModule
{
    public record CreateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid Id);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender mediator) =>
        {
            var command = new CreateProductCommand(request.Name, request.Categories, request.Description, request.ImageFile, request.Price);
            var result = await mediator.Send(command);
            var response = new CreateProductResponse(result.Id);
            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}
