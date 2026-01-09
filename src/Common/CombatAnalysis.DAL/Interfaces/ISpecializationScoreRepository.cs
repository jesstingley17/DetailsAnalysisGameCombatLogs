using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface ISpecializationScoreRepository
{
    Task<int> UpdateAsync(SpecializationScore item, CancellationToken cancellationToken);

    Task<SpecializationScore?> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);
}
