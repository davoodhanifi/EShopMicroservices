using BuildingBlocks.Services;

namespace Basket.Api.Data;

public class CachedBasketRepository(IBasketRepository repository, ICacheService cacheService) : IBasketRepository
{
    private readonly IBasketRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

    public async Task<ShoppingCart> Get(string username, CancellationToken cancellationToken = default)
    {
        return await _cacheService.GetOrAddAsync(username, () => _repository.Get(username, cancellationToken));
    }

    public async Task<ShoppingCart> Store(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        await _repository.Store(shoppingCart, cancellationToken);
        await _cacheService.AddAsync(shoppingCart.Username, shoppingCart);

        return shoppingCart;
    }

    public async Task<bool> Delete(string username, CancellationToken cancellationToken = default)
    {
        await _repository.Delete(username, cancellationToken);
        await _cacheService.RemoveAsync(username);

        return true;
    }
}
