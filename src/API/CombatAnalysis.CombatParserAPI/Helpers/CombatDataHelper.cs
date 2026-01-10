using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces.Entities;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Helpers;

public class CombatDataHelper(IMapper mapper, ILogger<CombatDataHelper> logger, ISpecializationScoreHelper specializationScoreHelper, IServiceScopeFactory serviceScopeFactory) : ICombatDataHelper
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatDataHelper> _logger = logger;
    private readonly ISpecializationScoreHelper _specializationScoreHelper = specializationScoreHelper;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public CombatDetails CreateCombatDetails(CombatModel combat)
    {
        var playersId = combat.CombatPlayers.Select(x => x.Player.GameId).ToList();

        var combatDetails = new CombatDetails(_logger, combat.PetsId);
        combatDetails.Calculate(playersId, combat.Data, combat.StartDate, combat.FinishDate);
        combatDetails.CalculateGeneralData(playersId, combat.Duration);

        return combatDetails;
    }

    public async Task CreateCombatPlayersDataAsync(CombatDetails combatDetails, List<CombatPlayerDto> combatPlayers, int combatId, CancellationToken cancellationToken)
    {
        var playersId = combatPlayers.Select(x => x.Player.GameId).ToList();

        var uploadTasks = combatPlayers.Select(item => UploadAsync(item, combatDetails, combatId, cancellationToken)).ToList();
        await Task.WhenAll(uploadTasks);

        var uploadCombatAuraTasks = combatDetails.Auras.Select(item => UploadCombatAuraData([.. item.Value.Select(x => x.Value)], combatId, cancellationToken)).ToList();
        await Task.WhenAll(uploadCombatAuraTasks);
    }

    public async Task UpdateSpecializationScoreAsync(List<CombatPlayerDto> combatPlayers, CombatDetails combatDetails, int bossId, CancellationToken cancellationToken)
    {
        var bestSpecScores = new List<BestSpecializationScoreDto?>();
        var specScores = new List<SpecializationScoreDto?>();
        foreach (var item in combatPlayers)
        {
            var specScore = await _specializationScoreHelper.GetSpecializationScoreAsync(item.Id, cancellationToken);
            specScores.Add(specScore);

            if (specScore != null)
            {
                var bestSpecScore = await _specializationScoreHelper.GetBestSpecializationScoreAsync(specScore.SpecializationId, bossId, cancellationToken);
                bestSpecScores.Add(bestSpecScore);
            }
            else
            {
                bestSpecScores.Add(null);
            }
        }

        var index = 0;
        foreach (var item in combatPlayers)
        {
            if (specScores[index] != null && bestSpecScores[index] != null)
            {
                await _specializationScoreHelper.UpdateSpecializationScoreAsync(item.DamageDone, item.HealDone, bestSpecScores[index]!, specScores[index]!, cancellationToken);
                await _specializationScoreHelper.UpdateBestSpecializationScoreAsync(item.DamageDone, item.HealDone, bestSpecScores[index]!, cancellationToken);
            }

            index++;
        }
    }

    private async Task UploadAsync(CombatPlayerDto combatPlayer, CombatDetails combatDetails, int combatId, CancellationToken cancellationToken)
    {
        foreach (var item in combatDetails.PlayersDeath[combatPlayer.Player.GameId])
        {
            if (combatDetails.DamageTaken[combatPlayer.Player.GameId].TryGetValue(item.Value.Username, out var lastDamageTaken))
            {
                item.Value.LastHitValue = lastDamageTaken.Value;
                item.Value.LastHitSpell = lastDamageTaken.Spell;
            }
        }

        var uploadTasks = new List<Task>
        {
            UploadCombatPlayerPositionData([.. combatDetails.Positions[combatPlayer.Player.GameId].Select(x => x.Value)], combatPlayer.Id, combatId, cancellationToken),

            UploadPlayerInfoBatch<DamageDone, DamageDoneDto>([.. combatDetails.DamageDone[combatPlayer.Player.GameId].Select(x => x.Value)], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<HealDone, HealDoneDto>([.. combatDetails.HealDone[combatPlayer.Player.GameId].Select(x => x.Value)], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<DamageTaken, DamageTakenDto>([.. combatDetails.DamageTaken[combatPlayer.Player.GameId].Select(x => x.Value)], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<ResourceRecovery, ResourceRecoveryDto>([.. combatDetails.ResourcesRecovery[combatPlayer.Player.GameId].Select(x => x.Value)], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<DamageDoneGeneral, DamageDoneGeneralDto>(combatDetails.DamageDoneGeneral[combatPlayer.Player.GameId], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<HealDoneGeneral, HealDoneGeneralDto>(combatDetails.HealDoneGeneral[combatPlayer.Player.GameId], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<DamageTakenGeneral, DamageTakenGeneralDto>(combatDetails.DamageTakenGeneral[combatPlayer.Player.GameId], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<ResourceRecoveryGeneral, ResourceRecoveryGeneralDto>(combatDetails.ResourcesRecoveryGeneral[combatPlayer.Player.GameId], combatPlayer.Id, cancellationToken),
            UploadPlayerInfoBatch<PlayerDeath, CombatPlayerDeathDto>([.. combatDetails.PlayersDeath[combatPlayer.Player.GameId].Select(x => x.Value)], combatPlayer.Id, cancellationToken),
        };

        await Task.WhenAll(uploadTasks);
    }

    private async Task UploadPlayerInfoBatch<TModel, TModelMap>(List<TModel> data, int combatPlayerId, CancellationToken cancellationToken)
        where TModel : class, ICombatPlayerEntity
        where TModelMap : class, BL.Interfaces.Entity.ICombatPlayerEntity
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<ICreateBatchService<TModelMap>>();

        data = [.. data.Select(x => 
        {
            x.CombatPlayerId = combatPlayerId;
            return x; 
        })];
        var map = _mapper.Map<List<TModelMap>>(data);

        await scopedService.CreateBatchAsync(map, cancellationToken);
    }

    private async Task UploadCombatPlayerPositionData(List<CombatPlayerPosition> combatPlayerPositions, int combatPlayerId, int combatId, CancellationToken cancellationToke)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<ICreateBatchService<CombatPlayerPositionDto>>();

        combatPlayerPositions = [.. combatPlayerPositions.Select(x =>
        {
            x.CombatId = combatId;
            x.CombatPlayerId = combatPlayerId;
            return x;
        })];
        var combatPlayerPositionsMap = _mapper.Map<List<CombatPlayerPositionDto>>(combatPlayerPositions);

        await scopedService.CreateBatchAsync(combatPlayerPositionsMap, cancellationToke);
    }

    private async Task UploadCombatAuraData(List<CombatAura> combatAuras, int combatId, CancellationToken cancellationToke)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<ICreateBatchService<CombatAuraDto>>();

        combatAuras = [.. combatAuras.Select(x =>
        {
            x.CombatId = combatId;
            return x;
        })];
        var combatAurasMap = _mapper.Map<List<CombatAuraDto>>(combatAuras);

        await scopedService.CreateBatchAsync(combatAurasMap, cancellationToke);
    }
}
