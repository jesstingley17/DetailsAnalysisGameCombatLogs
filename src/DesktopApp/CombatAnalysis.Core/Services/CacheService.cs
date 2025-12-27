using CombatAnalysis.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CombatAnalysis.Core.Services;

internal class CacheService(IMemoryCache cache) : ICacheService
{
    private readonly IMemoryCache _cache = cache;

    public void Add<TModel>(string key, TModel data, int expirationInMinutes = 30)
        where TModel : class
    {
        _cache.Set(key, data, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(expirationInMinutes)
        });
    }

    public TModel? Get<TModel>(string key)
        where TModel : class
    {
        _cache.TryGetValue(key, out TModel? data);
        return data;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
