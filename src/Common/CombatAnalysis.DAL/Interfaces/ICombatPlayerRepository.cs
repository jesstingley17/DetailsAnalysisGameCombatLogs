using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ICombatPlayerRepository
{
    Task CreateBatchAsync(IEnumerable<CombatPlayer> items);

    Task<CombatPlayer> CreateAsync(CombatPlayer item);

    Task<int> UpdateAsync(int id, CombatPlayer item);

    Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId);
}
