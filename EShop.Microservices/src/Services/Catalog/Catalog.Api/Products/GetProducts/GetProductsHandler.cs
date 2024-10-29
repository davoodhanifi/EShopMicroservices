namespace Catalog.Api.Products.CreateProduct;

public record GetProductsQuery(int PageNumber, int PageSize) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
    
public class GetProductsQueryByIdHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>()
                             .ToPagedListAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new GetProductsResult(products);
    }
}
