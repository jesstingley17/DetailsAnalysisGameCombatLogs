namespace CombatAnalysis.Core.Interfaces;

public interface ICacheService
{
    void Add<TModel>(string key, TModel data, int expirationInMinutes = 30) where TModel : class;

    TModel Get<TModel>(string key) where TModel : class;

    void Remove(string key);
}