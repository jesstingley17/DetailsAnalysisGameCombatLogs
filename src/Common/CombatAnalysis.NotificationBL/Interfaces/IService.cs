using CombatAnalysis.NotificationBL.DTO;
using System.Linq.Expressions;

namespace CombatAnalysis.NotificationBL.Interfaces;

public interface IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<TModel?> CreateAsync(TModel item);

    Task UpdateAsync(int id, TModel item);

    Task<bool> DeleteAsync(TIdType id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<IEnumerable<TModel>> GetByParamAsync<TValue>(Expression<Func<NotificationDto, TValue>> property, TValue value);

    Task<TModel?> GetByIdAsync(TIdType id);
}
