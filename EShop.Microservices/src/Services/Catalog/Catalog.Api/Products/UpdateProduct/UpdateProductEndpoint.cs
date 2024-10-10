namespace Catalog.Api.Products.UpdateProduct;

public record UpdateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
public record UpdateProductResponse(bool Result);

public class UpdateProductEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id}", async (Guid id, UpdateProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateProductCommand(id, request.Name, request.Categories,
                                                                    request.Description, request.ImageFile, request.Price));

            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        }).WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Product")
        .WithDescription("Update Product");

    }
}
