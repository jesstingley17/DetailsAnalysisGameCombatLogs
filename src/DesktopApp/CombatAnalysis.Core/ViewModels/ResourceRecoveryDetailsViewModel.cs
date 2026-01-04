using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.Core.ViewModels;

public class ResourceRecoveryDetailsViewModel : DetailsGenericTemplate<ResourceRecoveryModel, ResourceRecoveryGeneralModel>
{
    public ResourceRecoveryDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : base(httpClient, logger, mapper, cacheService, combatParserAPIService)
    {
        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 6);
    }

    protected override void GetDetailsFromCache(CombatPlayerModel? parameter)
    {
        var resourcesRecoveryCollection = _cacheService?.Get<Dictionary<string, List<ResourceRecovery>>>($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{SelectedCombat?.Number}");
        var resourcesRecoveryCollectionMap = _mapper.Map<List<ResourceRecoveryModel>>(resourcesRecoveryCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allDetailsInformations = [.. resourcesRecoveryCollectionMap];

        var resourcesRecoveryGeneralCollection = _cacheService?.Get<Dictionary<string, List<ResourceRecoveryGeneral>>>($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{SelectedCombat?.Number}");
        var resourcesRecoveryGeneralCollectionMap = _mapper.Map<List<ResourceRecoveryGeneralModel>>(resourcesRecoveryGeneralCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allGeneralInformations = [.. resourcesRecoveryGeneralCollectionMap];
    }
}
