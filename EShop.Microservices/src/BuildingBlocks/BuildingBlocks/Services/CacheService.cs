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
        var timeToLive = expirationIntervalInSeconds.HasValue
                ? TimeSpan.FromSeconds(expirationIntervalInSeconds.Value)
                : TimeSpan.FromSeconds(ExpirationIntervalInSeconds);

        await AddAsync(key, valueFromDb, timeToLive);
        return valueFromDb;
    }

    public async Task<bool> AddAsync<T>(string key, T value, TimeSpan timeToLive)
    {
        var json = JsonSerializer.Serialize(value);

        var result = await _database.StringSetAsync(key, json, timeToLive);

        return result;
    }

}