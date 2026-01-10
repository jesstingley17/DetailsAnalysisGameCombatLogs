using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageTakenDetailsViewModel : DetailsGenericTemplate<DamageTakenModel, DamageTakenGeneralModel>
{
    public DamageTakenDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper, 
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : base(httpClient, logger, mapper, cacheService, combatParserAPIService)
    {
        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 5);
    }

    protected override void GetDetailsFromCache(CombatPlayerModel? parameter)
    {
        var damageTakenCollection = _cacheService?.Get<ReadOnlyDictionary<string, ConcurrentDictionary<string, DamageTaken>>>($"{AppCacheKeys.CombatDetails_DamageTaken}_{SelectedCombat?.Number}");
        var damageTakenCollectionMap = _mapper.Map<ConcurrentDictionary<string, DamageTakenModel>>(damageTakenCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allDetailsInformations = [.. damageTakenCollectionMap.Values.OrderBy(x => x.Time)];

        var damageTakenGeneralCollection = _cacheService?.Get<ReadOnlyDictionary<string, List<DamageTakenGeneral>>>($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{SelectedCombat?.Number}");
        var damageTakenGeneralCollectionMap = _mapper.Map<List<DamageTakenGeneralModel>>(damageTakenGeneralCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allGeneralInformations = [.. damageTakenGeneralCollectionMap];
    }
}
