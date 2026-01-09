using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ICombatPlayerRepository
{
    Task CreateBatchAsync(IEnumerable<CombatPlayer> items, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
