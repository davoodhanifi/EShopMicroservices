namespace BuildingBlocks.Services;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key, Func<Task<T>> getFromSource, CancellationToken cancellationToken);
}
