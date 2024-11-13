using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BuildingBlocks.Services;
public class CacheService(IDistributedCache cache) : ICacheService
{
    private readonly IDistributedCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> getFromSource, CancellationToken cancellationToken)
    {
        var cachedValue = await _cache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrWhiteSpace(cachedValue))
        {
            var result = await getFromSource();
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(cachedValue), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10),
            }, cancellationToken);

            return result;
        }

        return JsonSerializer.Deserialize<T>(cachedValue)!;

    }
}
