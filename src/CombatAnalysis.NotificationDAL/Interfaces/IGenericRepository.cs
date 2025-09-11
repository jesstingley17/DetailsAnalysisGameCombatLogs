using System.Linq.Expressions;

namespace CombatAnalysis.NotificationDAL.Interfaces;

public interface IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(TIdType id);

    Task<TModel?> GetByIdAsync(TIdType id);

    Task<IEnumerable<TModel>> GetByParamAsync<TValue>(Expression<Func<TModel, TValue>> property, TValue value);

    Task<IEnumerable<TModel>> GetAllAsync();
}
