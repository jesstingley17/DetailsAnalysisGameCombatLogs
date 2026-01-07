using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerInfoRepository<TModel>
    where TModel : class, IEntity, ICombatPlayerEntity
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId);
}
