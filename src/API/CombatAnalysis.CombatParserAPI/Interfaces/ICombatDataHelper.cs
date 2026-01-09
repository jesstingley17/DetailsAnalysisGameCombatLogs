using CombatAnalysis.BL.DTO;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ICombatDataHelper
{
    CombatDetails CreateCombatDetails(CombatModel combat);

    Task CreateCombatPlayersDataAsync(CombatDetails combatDetails, List<CombatPlayerDto> combatPlayers, int combatId, CancellationToken cancellationToken);

    Task UpdateSpecializationScoreAsync(List<CombatPlayerDto> combatPlayers, CombatDetails combatDetails, int bossId, CancellationToken cancellationToken);
}
