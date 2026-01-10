using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IBestSpecializationScoreRepository
{
    Task<int> UpdateAsync(int id, BestSpecializationScore item, CancellationToken cancellationToken);

    Task<BestSpecializationScore?> GetAsync(int specializationId, int bossId, CancellationToken cancellationToken);
}
