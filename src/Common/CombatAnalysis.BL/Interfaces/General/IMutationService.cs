namespace CombatAnalysis.BL.Interfaces.General;

public interface IMutationService<TModel>
    where TModel : class
{
    Task<TModel> CreateAsync(TModel item, CancellationToken cancelationToken);

    Task<int> UpdateAsync(TModel item, CancellationToken cancelationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancelationToken);
}
