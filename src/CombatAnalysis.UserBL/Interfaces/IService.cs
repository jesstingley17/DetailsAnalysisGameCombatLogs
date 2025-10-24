using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Interfaces;

public interface IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<TModel?> CreateAsync(TModel item);

    Task UpdateAsync(TModel item);

    Task DeleteAsync(TIdType id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<IEnumerable<TModel>> GetByParamAsync<TValue>(Expression<Func<TModel, TValue>> property, TValue value);

    Task<TModel?> GetByIdAsync(TIdType id);
}
