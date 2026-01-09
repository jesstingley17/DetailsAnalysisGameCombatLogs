namespace CombatAnalysis.BL.Interfaces;

public interface ICreateBatchService<TModel>
        where TModel : class
{
    Task CreateBatchAsync(List<TModel> items, CancellationToken cancellationToken);
}
