using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISpecializationScoreHelper
{
    Task CreateSpecializationScoreAsync(List<CombatPlayerModel> combatPlayers, CombatDetails combatDetails);

    Task<SpecializationScoreDto?> GetSpecializationScoreAsync(int combatPlayerId);

    Task<BestSpecializationScoreDto?> GetBestSpecializationScoreAsync(int specId, int bossId);

    Task UpdateSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, SpecializationScoreDto specScore);

    Task UpdateBestSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore);
}
