using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ICombatDataHelper
{
    Task<CombatDetails> CreateCombatPlayersDataAsync(CombatModel combat);

    Task CreateSpecializationScoreAsync(List<CombatPlayerModel> combatPlayers, CombatDetails combatDetails, int bossId);
}
