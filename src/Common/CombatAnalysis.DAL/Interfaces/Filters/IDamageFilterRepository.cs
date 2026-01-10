using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces.Filters;

public interface IDamageFilterRepository
{
    Task<IEnumerable<List<CombatTarget>>> GetDamageByEachTargetAsync(int combatId, CancellationToken cancellationToken);
}
