namespace Basket.Api.Basket.GetBasket;

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart ShoppingCart);

public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        // Get ShoppingCart from Db

        return new GetBasketResult(new ShoppingCart("jhd"));
    }
}
