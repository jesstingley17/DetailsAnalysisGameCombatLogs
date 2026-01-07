namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerInfoPaginationService<TModel> : IPlayerInfoService<TModel>
    where TModel : class
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize);
}