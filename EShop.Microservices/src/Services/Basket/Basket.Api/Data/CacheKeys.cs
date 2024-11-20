namespace Basket.Api.Data;

public static class CacheKeys
{
    public static string Basket(string username) => $"basket-{username}";
}
