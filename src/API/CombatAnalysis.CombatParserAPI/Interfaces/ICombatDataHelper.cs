using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ICombatDataHelper
{
    CombatDetails CreateCombatDetails(CombatModel combat);

    Task CreateCombatPlayersDataAsync(CombatDetails combatDetails, CombatModel combat);

    Task UpdateSpecializationScoreAsync(List<CombatPlayerModel> combatPlayers, CombatDetails combatDetails, int bossId);
}
