using BuildingBlocks.Services;

namespace Basket.Api.Data;

public class CachedBasketRepository(IBasketRepository repository, ICacheService cacheService) : IBasketRepository
{
    private readonly IBasketRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICacheService _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    
    public async Task<ShoppingCart> Get(string username, CancellationToken cancellationToken = default)
    {
        return await _cacheService.GetAsync(username,
                                            () => _repository.Get(username, cancellationToken),
                                            cancellationToken);

    }

    public async Task<ShoppingCart> Store(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        return await _repository.Store(shoppingCart, cancellationToken);
    }

    public async Task<bool> Delete(string username, CancellationToken cancellationToken = default)
    {
        return await _repository.Delete(username, cancellationToken);
    }
}
