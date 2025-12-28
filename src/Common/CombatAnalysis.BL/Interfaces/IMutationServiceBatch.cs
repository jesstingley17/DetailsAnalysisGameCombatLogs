using CombatAnalysis.BL.Interfaces.General;

namespace CombatAnalysis.BL.Interfaces;

public interface IMutationServiceBatch<TModel> : IMutationService<TModel>
        where TModel : class
{
    Task CreateBatchAsync(List<TModel> items);
}
