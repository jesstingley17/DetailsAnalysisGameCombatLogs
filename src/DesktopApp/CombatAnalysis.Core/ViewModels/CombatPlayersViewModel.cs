using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Commands;

namespace CombatAnalysis.Core.ViewModels;

public class CombatPlayersViewModel : ParentTemplate<CombatModel>
{
    private readonly ICombatParserAPIService _combatParserAPIService;

    private int _selectedTabIndex = 1;
    private int _bestDamageDone;
    private int _bestHealDone;
    private int _bestDamageTaken;
    private int _bestResourcesRecovery;
    private CombatModel? _combat;
    private List<CombatPlayerModel>? _players;
    private List<CombatPlayerModel>? _mainPlayersCombat;
    private CombatPlayerModel? _selectedPlayer;
    private PlayerStatsModel? _selectedPlayerStats;
    private List<string>? _filterList;
    private int _combatInformationType;
    private int _selectedFilterIndex;
    private int _minDamageDone;
    private int _minHealDone;
    private int _minEnergyRecovery;
    private bool _openEditMinDamageDone;
    private bool _openEditMinHealDone;
    private bool _openEditMinEnergyRecovery;
    private bool _useFilterByMinDamageDone;
    private bool _useFilterByMinHealDone;
    private bool _useFilterByMinEnergyRecovery;
    private int _minDPS;
    private int _minHPS;
    private int _minRPS;
    private bool _openEditMinDPS;
    private bool _openEditMinHPS;
    private bool _openEditMinRPS;
    private bool _useFilterByMinDPS;
    private bool _useFilterByMinHPS;
    private bool _useFilterByMinRPS;
    private double _averageDamage;
    private double _averageHeal;
    private double _averageResources;
    private double _averageDamagePerSecond;
    private double _averageHealPerSecond;
    private double _averageResourcesPerSecond;
    private bool _showSummaryInformation;
    private int _totalDamage;
    private int _totalHeal;
    private int _totalResources;
    private double _totalDamagePerSecond;
    private double _totalHealPerSecond;
    private double _totalResourcesPerSecond;

    // Player stats modal window
    private bool _isModalOpen;
    private int _playerMainStat = -1;

    public CombatPlayersViewModel(ICombatParserAPIService combatParserAPIService)
    {
        _combatParserAPIService = combatParserAPIService;

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 2);

        CloseStatsCommand = new MvxCommand(CloseStats);

        OpenEditMinDamageDoneCommand = new MvxCommand(() => OpenEditMinDamageDone = true);
        ApplyMinDamageDoneCommand = new MvxCommand(ApplyMinDamageDone);
        OpenEditMinHealDoneCommand = new MvxCommand(() => OpenEditMinHealDone = true);
        ApplyMinHealDoneCommand = new MvxCommand(ApplyMinHealDone);
        OpenEditEnergyRecoveryCommand = new MvxCommand(() => OpenEditMinEnergyRecovery = true);
        ApplyMinEnergyRecoveryCommand = new MvxCommand(ApplyMinEnergyRecovery);

        OpenEditMinDPSCommand = new MvxCommand(() => OpenEditMinDPS = true);
        ApplyMinDPSCommand = new MvxCommand(ApplyMinDPS);
        OpenEditMinHPSCommand = new MvxCommand(() => OpenEditMinHPS = true);
        ApplyMinHPSCommand = new MvxCommand(ApplyMinHPS);
        OpenEditMinRPSCommand = new MvxCommand(() => OpenEditMinRPS = true);
        ApplyMinRPSCommand = new MvxCommand(ApplyMinRPS);

        UseFilterByMinDamageDoneCommand = new MvxCommand(() => UseFilterByMinDamageDone = !UseFilterByMinDamageDone);
        UseFilterByMinHealDoneCommand = new MvxCommand(() => UseFilterByMinHealDone = !UseFilterByMinHealDone);
        UseFilterByMinEnergyRecoveryCommand = new MvxCommand(() => UseFilterByMinEnergyRecovery = !UseFilterByMinEnergyRecovery);

        UseFilterByMinDPSCommand = new MvxCommand(() => UseFilterByMinDPS = !UseFilterByMinDPS);
        UseFilterByMinHPSCommand = new MvxCommand(() => UseFilterByMinHPS = !UseFilterByMinHPS);
        UseFilterByMinRPSCommand = new MvxCommand(() => UseFilterByMinRPS = !UseFilterByMinRPS);

        FilterClearCommand = new MvxCommand(ClearFilter);
    }

    #region Commands

    public IMvxCommand CloseStatsCommand { get; set; }

    public IMvxCommand OpenEditMinDamageDoneCommand { get; set; }

    public IMvxCommand ApplyMinDamageDoneCommand { get; set; }

    public IMvxCommand OpenEditMinHealDoneCommand { get; set; }

    public IMvxCommand ApplyMinHealDoneCommand { get; set; }

    public IMvxCommand OpenEditEnergyRecoveryCommand { get; set; }

    public IMvxCommand ApplyMinEnergyRecoveryCommand { get; set; }

    public IMvxCommand UseFilterByMinDamageDoneCommand { get; set; }

    public IMvxCommand UseFilterByMinHealDoneCommand { get; set; }

    public IMvxCommand UseFilterByMinEnergyRecoveryCommand { get; set; }

    public IMvxCommand OpenEditMinDPSCommand { get; set; }

    public IMvxCommand ApplyMinDPSCommand { get; set; }

    public IMvxCommand OpenEditMinHPSCommand { get; set; }

    public IMvxCommand ApplyMinHPSCommand { get; set; }

    public IMvxCommand OpenEditMinRPSCommand { get; set; }

    public IMvxCommand ApplyMinRPSCommand { get; set; }

    public IMvxCommand UseFilterByMinDPSCommand { get; set; }

    public IMvxCommand UseFilterByMinHPSCommand { get; set; }

    public IMvxCommand UseFilterByMinRPSCommand { get; set; }

    public IMvxCommand FilterClearCommand { get; set; }

    #endregion

    #region Player stats modal window properties

    public bool IsModalOpen
    {
        get => _isModalOpen;
        set => SetProperty(ref _isModalOpen, value);
    }

    public double ModalWidth { get; set; } = 600;

    public double ModalHeight { get; set; } = 500;

    public string ModalTitle { get; set; } = "Player Stats";

    public int PlayerMainStat
    {
        get => _playerMainStat;
        set => SetProperty(ref _playerMainStat, value);
    }

    #endregion

    #region View model properties

    public int SelectedTabIndex
    {
        get { return _selectedTabIndex; }
        set
        {
            SetProperty(ref _selectedTabIndex, value);
            OrderBy(value);
        }
    }

    public int BestDamageDone
    {
        get { return _bestDamageDone; }
        set
        {
            SetProperty(ref _bestDamageDone, value);
        }
    }

    public int BestHealDone
    {
        get { return _bestHealDone; }
        set
        {
            SetProperty(ref _bestHealDone, value);
        }
    }

    public int BestDamageTaken
    {
        get { return _bestDamageTaken; }
        set
        {
            SetProperty(ref _bestDamageTaken, value);
        }
    }

    public int BestResourcesRecovery
    {
        get { return _bestResourcesRecovery; }
        set
        {
            SetProperty(ref _bestResourcesRecovery, value);
        }
    }

    public List<CombatPlayerModel>? Players
    {
        get => _players;
        set
        {
            SetProperty(ref _players, value);

            if (value != null && value.Count > 0)
            {
                SelectedPlayer = value[0];
            }
        }
    }

    public CombatPlayerModel? SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            SetProperty(ref _selectedPlayer, value);

            if (value != null)
            {
                if (Basic is BasicTemplateViewModel basicTemplateViewModel)
                {
                    basicTemplateViewModel.Data = value;
                    basicTemplateViewModel.PetsId = (Combat?.PetsId) ?? [];
                }

                AsyncDispatcher.ExecuteOnMainThreadAsync(OpenStatsAsync);
            }
        }
    }

    public PlayerStatsModel? SelectedPlayerStats
    {
        get => _selectedPlayerStats;
        set
        {
            SetProperty(ref _selectedPlayerStats, value);
        }
    }

    public CombatModel? Combat
    {
        get => _combat;
        set
        {
            SetProperty(ref _combat, value);
        }
    }

    public int CombatInformationType
    {
        get { return _combatInformationType; }
        set
        {
            SetProperty(ref _combatInformationType, value);
        }
    }

    public List<string>? FilterList
    {
        get { return _filterList; }
        set
        {
            SetProperty(ref _filterList, value);
        }
    }

    public int SeletedFilterIndex
    {
        get { return _selectedFilterIndex; }
        set
        {
            SetProperty(ref _selectedFilterIndex, value);
            UseFilter(value);
        }
    }

    public int MinDamageDone
    {
        get { return _minDamageDone; }
        set
        {
            SetProperty(ref _minDamageDone, value);
        }
    }

    public int MinHealDone
    {
        get { return _minHealDone; }
        set
        {
            SetProperty(ref _minHealDone, value);
        }
    }

    public int MinEnergyRecovery
    {
        get { return _minEnergyRecovery; }
        set
        {
            SetProperty(ref _minEnergyRecovery, value);
        }
    }

    public bool OpenEditMinDamageDone
    {
        get { return _openEditMinDamageDone; }
        set
        {
            SetProperty(ref _openEditMinDamageDone, value);
        }
    }

    public bool OpenEditMinHealDone
    {
        get { return _openEditMinHealDone; }
        set
        {
            SetProperty(ref _openEditMinHealDone, value);
        }
    }

    public bool OpenEditMinEnergyRecovery
    {
        get { return _openEditMinEnergyRecovery; }
        set
        {
            SetProperty(ref _openEditMinEnergyRecovery, value);
        }
    }

    public bool UseFilterByMinDamageDone
    {
        get { return _useFilterByMinDamageDone; }
        set
        {
            SetProperty(ref _useFilterByMinDamageDone, value);
            if (!value)
            {
                MinDamageDone = 0;
                SeletedFilterIndex = 0;
                ApplyMinDamageDone();
            }
        }
    }

    public bool UseFilterByMinHealDone
    {
        get { return _useFilterByMinHealDone; }
        set
        {
            SetProperty(ref _useFilterByMinHealDone, value);
            if (!value)
            {
                MinHealDone = 0;
                SeletedFilterIndex = 0;
                ApplyMinHealDone();
            }
        }
    }

    public bool UseFilterByMinEnergyRecovery
    {
        get { return _useFilterByMinEnergyRecovery; }
        set
        {
            SetProperty(ref _useFilterByMinEnergyRecovery, value);
            if (!value)
            {
                MinEnergyRecovery = 0;
                SeletedFilterIndex = 0;
                ApplyMinEnergyRecovery();
            }
        }
    }

    public int MinDPS
    {
        get { return _minDPS; }
        set
        {
            SetProperty(ref _minDPS, value);
        }
    }

    public int MinHPS
    {
        get { return _minHPS; }
        set
        {
            SetProperty(ref _minHPS, value);
        }
    }

    public int MinRPS
    {
        get { return _minRPS; }
        set
        {
            SetProperty(ref _minRPS, value);
        }
    }

    public bool OpenEditMinDPS
    {
        get { return _openEditMinDPS; }
        set
        {
            SetProperty(ref _openEditMinDPS, value);
        }
    }

    public bool OpenEditMinHPS
    {
        get { return _openEditMinHPS; }
        set
        {
            SetProperty(ref _openEditMinHPS, value);
        }
    }

    public bool OpenEditMinRPS
    {
        get { return _openEditMinRPS; }
        set
        {
            SetProperty(ref _openEditMinRPS, value);
        }
    }

    public bool UseFilterByMinDPS
    {
        get { return _useFilterByMinDPS; }
        set
        {
            SetProperty(ref _useFilterByMinDPS, value);
            if (!value)
            {
                MinDPS = 0;
                SeletedFilterIndex = 0;
                ApplyMinDPS();
            }
        }
    }

    public bool UseFilterByMinHPS
    {
        get { return _useFilterByMinHPS; }
        set
        {
            SetProperty(ref _useFilterByMinHPS, value);
            if (!value)
            {
                MinHPS = 0;
                SeletedFilterIndex = 0;
                ApplyMinHPS();
            }
        }
    }

    public bool UseFilterByMinRPS
    {
        get { return _useFilterByMinRPS; }
        set
        {
            SetProperty(ref _useFilterByMinRPS, value);
            if (!value)
            {
                MinRPS = 0;
                SeletedFilterIndex = 0;
                ApplyMinRPS();
            }
        }
    }

    public double AverageDamage
    {
        get { return _averageDamage; }
        set
        {
            SetProperty(ref _averageDamage, value);
        }
    }

    public double AverageHeal
    {
        get { return _averageHeal; }
        set
        {
            SetProperty(ref _averageHeal, value);
        }
    }

    public double AverageResources
    {
        get { return _averageResources; }
        set
        {
            SetProperty(ref _averageResources, value);
        }
    }

    public double AverageDamagePerSecond
    {
        get { return _averageDamagePerSecond; }
        set
        {
            SetProperty(ref _averageDamagePerSecond, value);
        }
    }

    public double AverageHealPerSecond
    {
        get { return _averageHealPerSecond; }
        set
        {
            SetProperty(ref _averageHealPerSecond, value);
        }
    }

    public double AverageResourcesPerSecond
    {
        get { return _averageResourcesPerSecond; }
        set
        {
            SetProperty(ref _averageResourcesPerSecond, value);
        }
    }

    public int TotalDamage
    {
        get { return _totalDamage; }
        set
        {
            SetProperty(ref _totalDamage, value);
        }
    }

    public int TotalHeal
    {
        get { return _totalHeal; }
        set
        {
            SetProperty(ref _totalHeal, value);
        }
    }

    public int TotalResoures
    {
        get { return _totalResources; }
        set
        {
            SetProperty(ref _totalResources, value);
        }
    }

    public double TotalDamagePerSecond
    {
        get { return _totalDamagePerSecond; }
        set
        {
            SetProperty(ref _totalDamagePerSecond, value);
        }
    }

    public double TotalHealPerSecond
    {
        get { return _totalHealPerSecond; }
        set
        {
            SetProperty(ref _totalHealPerSecond, value);
        }
    }

    public double TotalResourcesPerSecond
    {
        get { return _totalResourcesPerSecond; }
        set
        {
            SetProperty(ref _totalResourcesPerSecond, value);
        }
    }

    public bool ShowSummaryInformation
    {
        get { return _showSummaryInformation; }
        set
        {
            SetProperty(ref _showSummaryInformation, value);
        }
    }

    #endregion

    public async Task OpenStatsAsync()
    {
        if (SelectedPlayer == null)
        {
            return;
        }

        SelectedPlayerStats = SelectedPlayer.Stats ?? await _combatParserAPIService.LoadCombatPlayerStatsAsync(SelectedPlayer.Id);

        SelectMainStat();

        IsModalOpen = true;
    }

    public void CloseStats()
    {
        SelectedPlayerStats = null;
        IsModalOpen = false;
    }

    public void ApplyMinDamageDone()
    {
        OpenEditMinDamageDone = !OpenEditMinDamageDone;

        if (MinDamageDone > 0)
        {
            FilterInformationByMinDamageDone(MinDamageDone);

            return;
        }

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
        if (MinHealDone > 0)
        {
            ApplyMinHealDone();
        }

        if (MinEnergyRecovery > 0)
        {
            ApplyMinEnergyRecovery();
        }
    }

    public void ApplyMinHealDone()
    {
        OpenEditMinHealDone = !OpenEditMinHealDone;

        if (MinHealDone > 0)
        {
            FilterInformationByMinHealDone(MinHealDone);

            return;
        }

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
        if (MinDamageDone > 0)
        {
            ApplyMinDamageDone();
        }

        if (MinEnergyRecovery > 0)
        {
            ApplyMinEnergyRecovery();
        }
    }

    public void ApplyMinEnergyRecovery()
    {
        OpenEditMinEnergyRecovery = !OpenEditMinEnergyRecovery;

        if (MinEnergyRecovery > 0)
        {
            FilterInformationByMinEnergyRecovery(MinEnergyRecovery);

            return;
        }

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
        if (MinDamageDone > 0)
        {
            ApplyMinDamageDone();
        }

        if (MinHealDone > 0)
        {
            ApplyMinHealDone();
        }
    }

    public void ApplyMinDPS()
    {
        if (MinDPS > 0)
        {
            FilterInformationByMinDPS(MinDPS);
            OpenEditMinDPS = false;

            return;
        }

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
        if (MinHPS > 0)
        {
            ApplyMinHPS();
        }

        if (MinRPS > 0)
        {
            ApplyMinRPS();
        }

        OpenEditMinDPS = false;
    }

    public void ApplyMinHPS()
    {
        if (MinHPS > 0)
        {
            FilterInformationByMinHPS(MinHPS);
            OpenEditMinHPS = false;

            return;
        }

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
        if (MinDPS > 0)
        {
            ApplyMinDPS();
        }

        if (MinRPS > 0)
        {
            ApplyMinRPS();
        }

        OpenEditMinHPS = false;
    }

    public void ApplyMinRPS()
    {
        if (MinRPS > 0)
        {
            FilterInformationByMinRPS(MinRPS);
            OpenEditMinRPS = false;

            return;
        }

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
        if (MinDPS > 0)
        {
            ApplyMinDPS();
        }

        if (MinHPS > 0)
        {
            ApplyMinHPS();
        }

        OpenEditMinRPS = false;
    }

    public override void ViewAppeared()
    {
        GetTotalValueFiltersName();

        base.ViewAppeared();
    }

    public override void Prepare(CombatModel parameter)
    {
        Combat = parameter;
        _mainPlayersCombat = parameter.CombatPlayers;

        Players = [.. parameter.CombatPlayers
            .Select(p => {
                var damageDonePercentages = (double)p.DamageDone / (double)parameter.DamageDone;
                p.DamageDonePercentages = double.Round(damageDonePercentages * 100, 2);

                var healDonePercentages = (double)p.HealDone / (double)parameter.HealDone;
                p.HealDonePercentages = double.Round(healDonePercentages * 100, 2);

                var damageTakenPercentages = (double)p.DamageTaken / (double)parameter.DamageTaken;
                p.DamageTakenPercentages = double.Round(damageTakenPercentages * 100, 2);

                var resourcesRecoveryPercentages = (double)p.ResourcesRecovery / (double)parameter.ResourcesRecovery;
                p.ResourcesRecoveryPercentages = double.Round(resourcesRecoveryPercentages * 100, 2);

                return p;
            })
            .OrderByDescending(p => p.DamageDone)];

        BestDamageDone = Players.Max(p => p.DamageDone);
        BestHealDone = Players.Max(p => p.HealDone);
        BestDamageTaken = Players.Max(p => p.DamageTaken);
        BestResourcesRecovery = Players.Max(p => p.ResourcesRecovery);

        SelectedPlayer = Players[0];

        var damageDone = Players.Average(x => x.DamageDone);
        var healDone = Players.Average(x => x.HealDone);
        var energyRecovery = Players.Average(x => x.ResourcesRecovery);

        AverageDamage = double.Round(damageDone, 2);
        AverageHeal = double.Round(healDone, 2);
        AverageResources = double.Round(energyRecovery, 2);

        AverageDamagePerSecond = Players.Average(x => x.DamageDonePerSecond);
        AverageHealPerSecond = Players.Average(x => x.HealDonePerSecond);
        AverageResourcesPerSecond = Players.Average(x => x.ResourcesRecoveryPerSecond);

        TotalDamage = Players.Sum(x => x.DamageDone);
        TotalHeal = Players.Sum(x => x.HealDone);
        TotalResoures = Players.Sum(x => x.ResourcesRecovery);

        TotalDamagePerSecond = Players.Sum(x => x.DamageDonePerSecond);
        TotalHealPerSecond = Players.Sum(x => x.HealDonePerSecond);
        TotalResourcesPerSecond = Players.Sum(x => x.ResourcesRecoveryPerSecond);
    }

    private void OrderBy(int tabindex)
    {
        if (Players == null)
        {
            return;
        }

        Players = tabindex switch
        {
            1 => [.. Players.OrderByDescending(p => p.DamageDone)],
            2 => [.. Players.OrderByDescending(p => p.HealDone)],
            3 => [.. Players.OrderByDescending(p => p.DamageTaken)],
            4 => [.. Players.OrderByDescending(p => p.ResourcesRecovery)],
            _ => [.. Players.OrderByDescending(p => p.DamageDone)],
        };
    }

    private void GetTotalValueFiltersName()
    {
        var minDamage = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatPlayers.Resource.MinDamage"];
        var minHeal = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatPlayers.Resource.MinHeal"];
        var minResurces = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.CombatPlayers.Resource.MinResources"];

        FilterList = ["No any", minDamage, minHeal, minResurces];
    }

    private void UseFilter(int index)
    {
        switch (index)
        {
            case 1:
                if (CombatInformationType == 0)
                {
                    UseFilterByMinDamageDone = true;
                }
                else
                {
                    UseFilterByMinDPS = true;
                }
                break;
            case 2:
                if (CombatInformationType == 0)
                {
                    UseFilterByMinHealDone = true;
                }
                else
                {
                    UseFilterByMinHPS = true;
                }
                break;
            case 3:
                if (CombatInformationType == 0)
                {
                    UseFilterByMinEnergyRecovery = true;
                }
                else
                {
                    UseFilterByMinRPS = true;
                }
                break;
            default:
                break;
        }
    }

    private void ClearFilter()
    {
        UseFilterByMinDamageDone = false;
        UseFilterByMinHealDone = false;
        UseFilterByMinEnergyRecovery = false;

        MinDamageDone = 0;
        MinHealDone = 0;
        MinEnergyRecovery = 0;

        Players = _mainPlayersCombat != null ? [.. _mainPlayersCombat] : [];
    }

    private void FilterInformationByMinDamageDone(int minDamageDone)
    {
        if (_mainPlayersCombat == null)
        {
            return;
        }

        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in _mainPlayersCombat)
        {
            if (player.DamageDone >= minDamageDone)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        Players = [.. temporaryPlayersCombat];
    }

    private void FilterInformationByMinHealDone(int minHealDone)
    {
        if (Players == null)
        {
            return;
        }

        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in Players)
        {
            if (player.HealDone >= minHealDone)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        Players = [.. temporaryPlayersCombat];
    }

    private void FilterInformationByMinEnergyRecovery(int minEnergyRecovery)
    {
        if (Players == null)
        {
            return;
        }

        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in Players)
        {
            if (player.ResourcesRecovery >= minEnergyRecovery)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        Players = [.. temporaryPlayersCombat];
    }

    private void FilterInformationByMinDPS(int minDPS)
    {
        if (Players == null)
        {
            return;
        }

        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in Players)
        {
            if (player.DamageDonePerSecond >= minDPS)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        Players = [.. temporaryPlayersCombat];
    }

    private void FilterInformationByMinHPS(int minHPS)
    {
        if (Players == null)
        {
            return;
        }

        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in Players)
        {
            if (player.HealDonePerSecond >= minHPS)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        Players = [.. temporaryPlayersCombat];
    }

    private void FilterInformationByMinRPS(int minRPS)
    {
        if (Players == null)
        {
            return;
        }

        var temporaryPlayersCombat = new List<CombatPlayerModel>();
        foreach (var player in Players)
        {
            if (player.ResourcesRecoveryPerSecond >= minRPS)
            {
                temporaryPlayersCombat.Add(player);
            }
        }

        Players = [.. temporaryPlayersCombat];
    }

    private void SelectMainStat()
    {
        if (SelectedPlayer == null || SelectedPlayerStats == null)
        {
            return;
        }

        PlayerMainStat = SelectedPlayerStats.Strength > SelectedPlayerStats.Intelligence && SelectedPlayerStats.Strength > SelectedPlayerStats.Agility
            ? 0 :
                SelectedPlayerStats.Intelligence > SelectedPlayerStats.Strength && SelectedPlayerStats.Intelligence > SelectedPlayerStats.Agility
                ? 2 : 1;
    }
}