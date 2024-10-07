namespace Catalog.Api.Products.CreateProduct;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
    
public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var result = await _session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(result);
    }
}
