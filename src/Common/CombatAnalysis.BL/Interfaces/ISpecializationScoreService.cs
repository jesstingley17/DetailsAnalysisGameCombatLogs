using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface ISpecializationScoreService
{
    Task<int> UpdateAsync(SpecializationScoreDto item);

    Task<bool> DeleteAsync(int id);

    Task<SpecializationScoreDto?> GetByCombatPlayerIdAsync(int combatPlayerId);
}
