using static Catalog.Api.Products.CreateProduct.GetProductsEndpoint;

namespace Catalog.Api.Products.GetProductByCategory;

public record GetProductByCategoryRequest();
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));
            var response = result.Adapt<GetProductByCategoryResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductByCategory")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category");
    }
}
