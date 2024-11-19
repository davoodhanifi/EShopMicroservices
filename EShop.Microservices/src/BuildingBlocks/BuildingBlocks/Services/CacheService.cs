using StackExchange.Redis;
using System.Text.Json;

namespace BuildingBlocks.Services;
public class CacheService(IDatabase database) : ICacheService
{
    private readonly IDatabase _database = database ?? throw new ArgumentNullException(nameof(database));
    private const int ExpirationIntervalInSeconds = 600;

    public async Task<T> GetOrAddAsync<T>(string key,
                                          Func<Task<T>> getFromDb,
                                          int? expirationIntervalInSeconds = null) where T : class
    {
        var value = await _database.StringGetAsync(key);
        if (!string.IsNullOrWhiteSpace(value))
        {
            return JsonSerializer.Deserialize<T>(value)!;
        }

        var valueFromDb = await getFromDb();

        await AddAsync(key, valueFromDb, expirationIntervalInSeconds);
        return valueFromDb;
    }

    public async Task<bool> AddAsync<T>(string key, T value, int? expirationIntervalInSeconds = null)
    {
        var json = JsonSerializer.Serialize(value);
        var timeToLive = expirationIntervalInSeconds.HasValue
                       ? TimeSpan.FromSeconds(expirationIntervalInSeconds.Value)
                       : TimeSpan.FromSeconds(ExpirationIntervalInSeconds);

        return await _database.StringSetAsync(key, json, timeToLive);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}