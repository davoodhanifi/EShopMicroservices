namespace BuildingBlocks.Services;

public interface ICacheService
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> getFromDb, int? expirationIntervalInSeconds = null) where T : class;
    Task<bool> AddAsync<T>(string key, T value, int? expirationIntervalInSeconds = null);
    Task<bool> RemoveAsync(string key);
}
