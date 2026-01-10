using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface ISpecializationScoreService
{
    Task<int> UpdateAsync(SpecializationScoreDto item, CancellationToken cancellationToken);

    Task<SpecializationScoreDto?> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);
}
