using Basket.Api.Data;
using Basket.Api.Exceptions;

namespace Basket.Api.Basket.GetBasket;

public record GetBasketQuery(string Username) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart ShoppingCart);

public class GetBasketHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    private readonly IBasketRepository _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basketResult = await _basketRepository.Get(query.Username, cancellationToken);

        if (basketResult.IsError)
        {
            throw new BasketNotFoundException(query.Username);
        }

        var shoppingCart = basketResult.Value;
        return new GetBasketResult(shoppingCart);
    }
}
