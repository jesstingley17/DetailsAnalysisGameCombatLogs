using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Logging;

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
        var healDoneCollection = _cacheService?.GetDataFromCache<Dictionary<string, List<HealDone>>>($"{AppCacheKeys.CombatDetails_HealDone}_{SelectedCombat?.LocallyNumber}");
        var healDoneCollectionMap = _mapper.Map<List<HealDoneModel>>(healDoneCollection?[parameter != null ? parameter.PlayerId : string.Empty]);
        _allDetailsInformations = [.. healDoneCollectionMap];

        var healDoneGeneralCollection = _cacheService?.GetDataFromCache<Dictionary<string, List<HealDoneGeneral>>>($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{SelectedCombat?.LocallyNumber}");
        var healDoneGeneralCollectionMap = _mapper.Map<List<HealDoneGeneralModel>>(healDoneGeneralCollection?[parameter != null ? parameter.PlayerId : string.Empty]);
        _allGeneralInformations = [.. healDoneGeneralCollectionMap];
    }
}
