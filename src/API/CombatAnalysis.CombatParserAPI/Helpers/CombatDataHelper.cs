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

    public async Task CreateCombatPlayersDataAsync(CombatDetails combatDetails, List<CombatPlayerDto> combatPlayers, int combatId)
    {
        var playersId = combatPlayers.Select(x => x.Player.GameId).ToList();

        var uploadTasks = combatPlayers.Select(item => UploadAsync(item, combatDetails, combatId)).ToList();
        await Task.WhenAll(uploadTasks);

        var uploadCombatAuraTasks = combatDetails.Auras.Select(item => UploadCombatAuraData(item.Value, combatId)).ToList();
        await Task.WhenAll(uploadCombatAuraTasks);
    }

    public async Task UpdateSpecializationScoreAsync(List<CombatPlayerDto> combatPlayers, CombatDetails combatDetails, int bossId)
    {
        var bestSpecScores = new List<BestSpecializationScoreDto?>();
        var specScores = new List<SpecializationScoreDto?>();
        foreach (var item in combatPlayers)
        {
            var specScore = await _specializationScoreHelper.GetSpecializationScoreAsync(item.Id);
            specScores.Add(specScore);

            if (specScore != null)
            {
                var bestSpecScore = await _specializationScoreHelper.GetBestSpecializationScoreAsync(specScore.SpecializationId, bossId);
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
                await _specializationScoreHelper.UpdateSpecializationScoreAsync(item.DamageDone, item.HealDone, bestSpecScores[index]!, specScores[index]!);
                await _specializationScoreHelper.UpdateBestSpecializationScoreAsync(item.DamageDone, item.HealDone, bestSpecScores[index]!);
            }

            index++;
        }
    }

    private async Task UploadAsync(CombatPlayerDto combatPlayer, CombatDetails combatDetails, int combatId)
    {
        foreach (var item in combatDetails.PlayersDeath[combatPlayer.Player.GameId])
        {
            var lastDamageTaken = combatDetails.DamageTaken[combatPlayer.Player.GameId].LastOrDefault(x => x.Target == item.Username);
            if (lastDamageTaken != null)
            {
                item.LastHitValue = lastDamageTaken.Value;
                item.LastHitSpell = lastDamageTaken.Spell;
            }
        }

        var uploadTasks = new List<Task>
        {
            UploadCombatPlayerPositionData(combatDetails.Positions[combatPlayer.Player.GameId], combatPlayer.Id, combatId),

            UploadPlayerInfoBatch<DamageDone, DamageDoneDto>(combatDetails.DamageDone[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<HealDone, HealDoneDto>(combatDetails.HealDone[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<DamageTaken, DamageTakenDto>(combatDetails.DamageTaken[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<ResourceRecovery, ResourceRecoveryDto>(combatDetails.ResourcesRecovery[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<DamageDoneGeneral, DamageDoneGeneralDto>(combatDetails.DamageDoneGeneral[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<HealDoneGeneral, HealDoneGeneralDto>(combatDetails.HealDoneGeneral[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<DamageTakenGeneral, DamageTakenGeneralDto>(combatDetails.DamageTakenGeneral[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<ResourceRecoveryGeneral, ResourceRecoveryGeneralDto>(combatDetails.ResourcesRecoveryGeneral[combatPlayer.Player.GameId], combatPlayer.Id),
            UploadPlayerInfoBatch<PlayerDeath, CombatPlayerDeathDto>(combatDetails.PlayersDeath[combatPlayer.Player.GameId], combatPlayer.Id),
        };

        await Task.WhenAll(uploadTasks);
    }

    private async Task UploadPlayerInfoBatch<TModel, TModelMap>(List<TModel> data, int combatPlayerId)
        where TModel : class, ICombatPlayerEntity
        where TModelMap : class, BL.Interfaces.Entity.ICombatPlayerEntity
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationServiceBatch<TModelMap>>();

        data = [.. data.Select(x => 
        {
            x.CombatPlayerId = combatPlayerId;
            return x; 
        })];
        var map = _mapper.Map<List<TModelMap>>(data);

        await scopedService.CreateBatchAsync(map);
    }

    private async Task UploadCombatPlayerPositionData(List<CombatPlayerPosition> combatPlayerPositions, int combatPlayerId, int combatId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationServiceBatch<CombatPlayerPositionDto>>();

        combatPlayerPositions = [.. combatPlayerPositions.Select(x =>
        {
            x.CombatId = combatId;
            x.CombatPlayerId = combatPlayerId;
            return x;
        })];
        var combatPlayerPositionsMap = _mapper.Map<List<CombatPlayerPositionDto>>(combatPlayerPositions);

        await scopedService.CreateBatchAsync(combatPlayerPositionsMap);
    }

    private async Task UploadCombatAuraData(List<CombatAura> combatAuras, int combatId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationServiceBatch<CombatAuraDto>>();

        combatAuras = [.. combatAuras.Select(x =>
        {
            x.CombatId = combatId;
            return x;
        })];
        var combatAurasMap = _mapper.Map<List<CombatAuraDto>>(combatAuras);

        await scopedService.CreateBatchAsync(combatAurasMap);
    }
}
