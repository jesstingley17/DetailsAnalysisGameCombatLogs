using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageDoneDetailsViewModel : DetailsGenericTemplate<DamageDoneModel, DamageDoneGeneralModel>
{
    private bool _isShowPets = true;

    public DamageDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : base(httpClient, logger, mapper, cacheService, combatParserAPIService)
    {
        ShowPetsCommand = new MvxCommand(ShowPets);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 3);
    }

    public IMvxCommand ShowPetsCommand { get; private set; }

    #region Properties

    public bool IsShowPets
    {
        get { return _isShowPets; }
        set
        {
            SetProperty(ref _isShowPets, value);
        }
    }

    #endregion

    protected override void GetDetailsFromCache(CombatPlayerModel? parameter)
    {
        var damageDoneCollection = _cacheService?.Get<ReadOnlyDictionary<string, ConcurrentDictionary<string, DamageDone>>>($"{AppCacheKeys.CombatDetails_DamageDone}_{SelectedCombat?.Number}");
        var damageDoneCollectionMap = _mapper.Map<ConcurrentDictionary<string, DamageDoneModel>>(damageDoneCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allDetailsInformations = [.. damageDoneCollectionMap.Values.OrderBy(x => x.Time)];

        var damageDoneGeneralCollection = _cacheService?.Get<ReadOnlyDictionary<string, List<DamageDoneGeneral>>>($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{SelectedCombat?.Number}");
        var damageDoneGeneralCollectionMap = _mapper.Map<List<DamageDoneGeneralModel>>(damageDoneGeneralCollection?[parameter != null ? parameter.Player.GameId : string.Empty]);
        _allGeneralInformations = [.. damageDoneGeneralCollectionMap];
    }

    private void ShowPets()
    {
        if (_allGeneralInformations == null || _allDetailsInformations == null)
        {
            return;
        }

        if (!IsShowPets)
        {
            var generalWithoutPets = _allGeneralInformations.Where(x => !x.IsPet);
            GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(generalWithoutPets);

            var detailsWithoutPets = _allDetailsInformations.Where(x => !x.IsPet);
            DetailsInformations = new ObservableCollection<DamageDoneModel>(detailsWithoutPets);
        }
        else
        {
            GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(_allGeneralInformations);
            DetailsInformations = new ObservableCollection<DamageDoneModel>(_allDetailsInformations);

            LoadGeneralDetailsFromCache();
            LoadDetailsFromCache(Page, _pageSize);
        }

        TotalValue = GeneralInformations.Sum(x => x.Value);

        GetSources();
    }
}