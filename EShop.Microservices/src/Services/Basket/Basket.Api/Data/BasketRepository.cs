using Basket.Api.Exceptions;

namespace Basket.Api.Data;

public class BasketRepository (IDocumentSession session) : IBasketRepository
{
    private readonly IDocumentSession _session = session ?? throw new ArgumentNullException(nameof(session));
        
    public async Task<ShoppingCart> Get(string username, CancellationToken cancellationToken = default)
    {
        var basket = await _session.LoadAsync<ShoppingCart>(username, cancellationToken);
        //return basket is null ? Error.NotFound($"{username}") : basket;
        return basket is null ? throw new BasketNotFoundException(username) : basket;
    }

    public async Task<ShoppingCart> Store(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        _session.Store(shoppingCart);
        await _session.SaveChangesAsync(cancellationToken);
        return shoppingCart;
    }

    public async Task<bool> Delete(string username, CancellationToken cancellationToken = default)
    {
        _session.Delete<ShoppingCart>(username);
        await _session.SaveChangesAsync(cancellationToken);
        return true;
    }
}
