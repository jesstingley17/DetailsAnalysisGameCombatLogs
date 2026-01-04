using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ISpecializationScoreRepository
{
    Task CreateBatchAsync(IEnumerable<SpecializationScore> items);

    Task<int> UpdateAsync(SpecializationScore item);

    Task<bool> DeleteAsync(int id);

    Task<SpecializationScore?> GetByCombatPlayerIdAsync(int combatPlayerId);
}
