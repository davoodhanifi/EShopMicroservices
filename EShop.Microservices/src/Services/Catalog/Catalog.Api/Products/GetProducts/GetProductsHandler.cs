namespace Catalog.Api.Products.CreateProduct;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
    
public class GetProductsQueryByIdHandler(IDocumentSession session, ILogger<GetProductsQueryByIdHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<GetProductsQueryByIdHandler> _logger = logger;

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"GetProductsQueryByIdHandler.Handle called with query: {query}");
        var products = await _session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}
