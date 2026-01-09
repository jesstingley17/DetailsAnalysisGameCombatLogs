using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParserAPI.Interfaces;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal class SpecializationScoreHelper(ISpecializationScoreService service, IBestSpecializationScoreService bestScoreService, ISpecializationService specService) : ISpecializationScoreHelper
{
    private readonly ISpecializationScoreService _service = service;
    private readonly IBestSpecializationScoreService _bestScoreService = bestScoreService;
    private readonly ISpecializationService _specService = specService;

    public async Task CreateSpecializationScoreAsync(CombatPlayerDto combatPlayer, CombatDetails combatDetails, CancellationToken cancellationToken)
    {
        var spellIds = combatPlayer.DamageDone > combatPlayer.HealDone
            ? combatDetails.DamageDoneGeneral[combatPlayer.Player.GameId].Select(d => d.GameSpellId).ToArray()
            : [.. combatDetails.HealDoneGeneral[combatPlayer.Player.GameId].Select(d => d.GameSpellId)];
        var spellsIdsStr = string.Join(',', spellIds);

        var spec = await _specService.GetBySpellsAsync(spellsIdsStr, cancellationToken);
        if (spec == null)
        {
            return;
        }

        var score = new SpecializationScoreDto
        {
            DamageDone = combatPlayer.DamageDone,
            HealDone = combatPlayer.HealDone,
            SpecializationId = spec.Id,
            CombatPlayerId = combatPlayer.Id,
        };

        combatPlayer.Score = score;
    }

    public async Task<SpecializationScoreDto?> GetSpecializationScoreAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var specScores = await _service.GetByCombatPlayerIdAsync(combatPlayerId, cancellationToken);

        return specScores;
    }

    public async Task<BestSpecializationScoreDto?> GetBestSpecializationScoreAsync(int specId, int bossId, CancellationToken cancellationToken)
    {
        var bestScore = await _bestScoreService.GetAsync(specId, bossId, cancellationToken);

        return bestScore;
    }

    public async Task UpdateSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, SpecializationScoreDto specScore, CancellationToken cancellationToken)
    {
        if (bestScore.DamageDone < damageDone)
        {
            specScore.DamageScore = 100;
        }
        else
        {
            specScore.DamageScore = damageDone == 0 ? 0 : ((double)damageDone / (double)bestScore.DamageDone) * 100;
        }

        if (bestScore.HealDone < healDone)
        {
            specScore.HealScore = 100;
        }
        else
        {
            specScore.HealScore = healDone == 0 ? 0 : ((double)healDone / (double)bestScore.HealDone) * 100;
        }

        specScore.Updated = DateTimeOffset.UtcNow;
        await _service.UpdateAsync(specScore, cancellationToken);
    }

    public async Task UpdateBestSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, CancellationToken cancellationToken)
    {
        var bestSpecScoreMustBeUpdated = false;
        var updatedBestScore = new BestSpecializationScoreDto
        {
            Id = bestScore.Id,
            SpecializationId = bestScore.SpecializationId,
            BossId = bestScore.BossId,
        };

        if (bestScore.DamageDone < damageDone)
        {
            updatedBestScore.DamageDone = damageDone;
            bestSpecScoreMustBeUpdated = true;
        }

        if (bestScore.HealDone < healDone)
        {
            updatedBestScore.HealDone = healDone;
            bestSpecScoreMustBeUpdated = true;
        }

        if (bestSpecScoreMustBeUpdated)
        {
            updatedBestScore.Updated = DateTimeOffset.UtcNow;
            await _bestScoreService.UpdateAsync(updatedBestScore, cancellationToken);
        }
    }
}
