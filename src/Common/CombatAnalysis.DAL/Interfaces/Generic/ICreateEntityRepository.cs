using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Generic;

public interface ICreateEntityRepository<TModel> : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    Task<TModel?> CreateAsync(TModel item, CancellationToken cancelationToken);
}
