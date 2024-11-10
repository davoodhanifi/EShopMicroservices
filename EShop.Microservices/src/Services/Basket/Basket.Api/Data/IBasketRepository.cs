namespace Basket.Api.Data;

public interface IBasketRepository
{
    Task<ErrorOr<ShoppingCart>> Get(string username, CancellationToken cancellationToken = default);
    Task<ShoppingCart> Store(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    Task<bool> Delete(string username, CancellationToken cancellationToken = default);
}
