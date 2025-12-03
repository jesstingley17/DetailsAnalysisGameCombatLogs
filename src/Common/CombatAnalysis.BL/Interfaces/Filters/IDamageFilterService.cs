using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces.Filters;

public interface IDamageFilterService
{
    Task<IEnumerable<List<CombatTargetDto>>> GetDamageByEachTargetAsync(int combatId);
}
