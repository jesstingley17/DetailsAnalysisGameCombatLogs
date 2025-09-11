using System.Linq.Expressions;

namespace CombatAnalysis.ChatBL.Interfaces;

public interface IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(TIdType id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<IEnumerable<TModel>> GetByParamAsync<TValue>(Expression<Func<TModel, TValue>> property, TValue value);

    Task<TModel> GetByIdAsync(TIdType id);
}
