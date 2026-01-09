namespace CombatAnalysis.BL.Interfaces.General;

public interface IQueryService<TModel>
    where TModel : class
{
    Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value, CancellationToken cancellationToken);

    Task<TModel> GetByIdAsync(int id, CancellationToken cancellationToken);
}
