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

public class HealDoneDetailsViewModel : DetailsGenericTemplate<HealDoneModel, HealDoneGeneralModel>
{
    public HealDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : base(httpClient, logger, mapper, cacheService, combatParserAPIService)
    {
        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 4);
    }

    protected override void GetDetailsFromCache(CombatPlayerModel? parameter)
    {
        var healDoneCollection = _cacheService?.Get<ReadOnlyDictionary<string, ConcurrentDictionary<string, HealDone>>>($"{AppCacheKeys.CombatDetails_HealDone}_{SelectedCombat?.Number}");
        var healDoneCollectionMap = _mapper.Map<ConcurrentDictionary<string, HealDoneModel>>(healDoneCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allDetailsInformations = [.. healDoneCollectionMap.Values.OrderBy(x => x.Time)];

        var healDoneGeneralCollection = _cacheService?.Get<ReadOnlyDictionary<string, List<HealDoneGeneral>>>($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{SelectedCombat?.Number}");
        var healDoneGeneralCollectionMap = _mapper.Map<List<HealDoneGeneralModel>>(healDoneGeneralCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allGeneralInformations = [.. healDoneGeneralCollectionMap];
    }
}
