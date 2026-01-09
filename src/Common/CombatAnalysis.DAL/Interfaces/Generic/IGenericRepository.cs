using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Generic;

public interface IGenericRepository<TModel>
    where TModel : class, IEntity
{
    Task<int> UpdateAsync(TModel item, CancellationToken cancelationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancelationToken);

    Task<TModel?> GetByIdAsync(int id, CancellationToken cancelationToken);

    Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value, CancellationToken cancelationToken);

    Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancelationToken);
}