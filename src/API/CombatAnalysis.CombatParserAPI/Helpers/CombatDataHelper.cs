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

public class CombatDataHelper(IMapper mapper, ILogger<CombatDataHelper> logger, IPlayerParseInfoHelper playerParseInfoHelper, IServiceScopeFactory serviceScopeFactory) : ICombatDataHelper
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CombatDataHelper> _logger = logger;
    private readonly IPlayerParseInfoHelper _playerParseInfoHelper = playerParseInfoHelper;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async Task SaveCombatPlayerAsync(CombatModel combat)
    {
        var parsedCombat = _mapper.Map<Combat>(combat);

        var playersId = combat.Players.Select(x => x.PlayerId).ToList();

        var combatDetails = new CombatDetails(_logger, combat.PetsId);
        combatDetails.Calculate(playersId, combat.Data, combat.StartDate, combat.FinishDate);
        combatDetails.CalculateGeneralData(playersId, combat.Duration);

        var uploadTasks = combat.Players.Select(item => UploadAsync(parsedCombat, item, combatDetails, combat.Id)).ToList();
        await Task.WhenAll(uploadTasks);

        var uploadCombatAuraTasks = combatDetails.Auras.Select(item => UploadCombatAuraData(item.Value, combat.Id)).ToList();
        await Task.WhenAll(uploadCombatAuraTasks);
    }

    private async Task UploadAsync(Combat combat, CombatPlayerModel combatPlayer, CombatDetails combatDetails, int combatId)
    {
        foreach (var item in combatDetails.PlayersDeath[combatPlayer.PlayerId])
        {
            var lastDamageTaken = combatDetails.DamageTaken[combatPlayer.PlayerId].LastOrDefault(x => x.Target == item.Username);
            if (lastDamageTaken != null)
            {
                item.LastHitValue = lastDamageTaken.Value;
                item.LastHitSpell = lastDamageTaken.Spell;
            }
        }

        var uploadTasks = new List<Task>
        {
            UploadCombatPlayerPositionData(combatDetails.Positions[combatPlayer.PlayerId], combatPlayer.Id, combatId),

            UploadPlayerInfoBatch<DamageDone, DamageDoneDto>(combatDetails.DamageDone[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<HealDone, HealDoneDto>(combatDetails.HealDone[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<DamageTaken, DamageTakenDto>(combatDetails.DamageTaken[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<ResourceRecovery, ResourceRecoveryDto>(combatDetails.ResourcesRecovery[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<DamageDoneGeneral, DamageDoneGeneralDto>(combatDetails.DamageDoneGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<HealDoneGeneral, HealDoneGeneralDto>(combatDetails.HealDoneGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<DamageTakenGeneral, DamageTakenGeneralDto>(combatDetails.DamageTakenGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<ResourceRecoveryGeneral, ResourceRecoveryGeneralDto>(combatDetails.ResourcesRecoveryGeneral[combatPlayer.PlayerId], combatPlayer.Id),
            UploadPlayerInfoBatch<PlayerDeath, PlayerDeathDto>(combatDetails.PlayersDeath[combatPlayer.PlayerId], combatPlayer.Id),
        };

        await Task.WhenAll(uploadTasks);

        //if (combat.IsWin)
        //{
        //    await _playerParseInfoHelper.UploadPlayerParseInfoAsync(combat, combatPlayer, combatDetails.DamageDoneGeneral[combatPlayer.PlayerId], combatDetails.HealDoneGeneral[combatPlayer.PlayerId]);
        //}
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
