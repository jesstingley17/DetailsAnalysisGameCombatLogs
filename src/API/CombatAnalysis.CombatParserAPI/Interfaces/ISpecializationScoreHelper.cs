using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Details;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISpecializationScoreHelper
{
    Task CreateSpecializationScoreAsync(CombatPlayerDto combatPlayer, CombatDetails combatDetails, CancellationToken cancellationToken);

    Task<SpecializationScoreDto?> GetSpecializationScoreAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<BestSpecializationScoreDto?> GetBestSpecializationScoreAsync(int specId, int bossId, CancellationToken cancellationToken);

    Task UpdateSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, SpecializationScoreDto specScore, CancellationToken cancellationToken);

    Task UpdateBestSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, CancellationToken cancellationToken);
}
