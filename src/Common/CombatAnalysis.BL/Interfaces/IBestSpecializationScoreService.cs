using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface IBestSpecializationScoreService
{
    Task<int> UpdateAsync(BestSpecializationScoreDto item);

    Task<BestSpecializationScoreDto?> GetAsync(int specializationId, int bossId);
}
