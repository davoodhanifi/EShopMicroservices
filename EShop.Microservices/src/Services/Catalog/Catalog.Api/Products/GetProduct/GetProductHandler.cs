namespace Catalog.Api.Products.CreateProduct;

public record GetProductQuery(Guid Id) : IQuery<GetProductResult>;

public record GetProductResult(Product Product);
    
public class GetProductQueryHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var result = await _session.LoadAsync<Product>(query.Id, cancellationToken);
        
        return new GetProductResult(result);
    }
}
