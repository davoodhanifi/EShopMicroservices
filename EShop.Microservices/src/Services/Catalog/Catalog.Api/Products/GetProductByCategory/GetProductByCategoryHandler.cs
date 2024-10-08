namespace Catalog.Api.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category): IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryHandler (IDocumentSession session, ILogger<GetProductByCategoryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<GetProductByCategoryHandler> _logger = logger;

    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"GetProductByCategoryHandler.Handle called with query: {query}");
        var products = await _session.Query<Product>()
                                   .Where(p => p.Categories.Contains(query.Category))
                                   .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}

