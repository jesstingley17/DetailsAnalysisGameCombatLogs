using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Generic;

public interface IGenericRepositoryBatch<TModel> : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    Task CreateBatchAsync(IEnumerable<TModel> items);
}
