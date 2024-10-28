namespace Catalog.Api.Products.CreateProduct;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
    
public class GetProductsQueryByIdHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}
