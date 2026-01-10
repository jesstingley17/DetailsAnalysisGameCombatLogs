using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.CombatPlayers;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;

namespace CombatAnalysis.Core.ViewModels;

public class CombatPlayersViewModel : ParentTemplate<CombatModel>
{
    private readonly ICombatParserAPIService _combatparserAPIService;

    private int _selectedTabIndex = 1;
    private CombatModel? _combat;
    private List<CombatPlayerModel>? _players;
    private List<CombatPlayerModel>? _mainPlayersCombat;
    private CombatPlayerModel? _selectedPlayer;

    public CombatPlayersViewModel(ICombatParserAPIService combatparserAPIService)
    {
        _combatparserAPIService = combatparserAPIService;

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 2);

        DamageDoneScoreVM = new DamageDoneScoreViewModel();
        DamageTakenScoreVM = new DamageTakenScoreViewModel();
        HealDoneScoreVM = new HealDoneScoreViewModel();
        ResourcesRecoveryScoreVM = new ResourcesRecoveryScoreViewModel();
        PlayerInfoVM = new PlayerInfoViewModel();
    }

    #region View model properties

    public int SelectedTabIndex
    {
        get { return _selectedTabIndex; }
        set
        {
            SetProperty(ref _selectedTabIndex, value);
            if (value > 0)
            {
                OrderBy(value);
            }
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
            }
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

    #endregion

    public DamageDoneScoreViewModel DamageDoneScoreVM { get; }

    public DamageTakenScoreViewModel DamageTakenScoreVM { get; }

    public HealDoneScoreViewModel HealDoneScoreVM { get; }

    public PlayerInfoViewModel PlayerInfoVM { get; }

    public ResourcesRecoveryScoreViewModel ResourcesRecoveryScoreVM { get; }

    public override void Prepare(CombatModel parameter)
    {
        Combat = parameter;
        if (Combat != null && Combat.CombatPlayers.Count != 0)
        {
            _mainPlayersCombat = [.. Combat.CombatPlayers];

            InitCombatPlayersData(_mainPlayersCombat);
        } 
    }

    public override async Task Initialize()
    {
        if (Combat == null || Combat.CombatPlayers.Count != 0)
        {
            return;
        }

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var combatPlayers = await _combatparserAPIService.LoadCombatPlayersAsync(Combat.Id, token);
        _mainPlayersCombat = [.. combatPlayers];

        InitCombatPlayersData(_mainPlayersCombat);

        await base.Initialize();
    }

    private void InitCombatPlayersData(List<CombatPlayerModel> combatPlayers)
    {
        if (Combat == null || combatPlayers.Count == 0)
        {
            return;
        }

        Players = [.. combatPlayers
            .Select(p => {
                var damageDonePercentages = (double)p.DamageDone / (double)Combat.DamageDone;
                p.DamageDonePercentages = double.Round(damageDonePercentages * 100, 2);

                var healDonePercentages = (double)p.HealDone / (double)Combat.HealDone;
                p.HealDonePercentages = double.Round(healDonePercentages * 100, 2);

                var damageTakenPercentages = (double)p.DamageTaken / (double)Combat.DamageTaken;
                p.DamageTakenPercentages = double.Round(damageTakenPercentages * 100, 2);

                var resourcesRecoveryPercentages = (double)p.ResourcesRecovery / (double)Combat.ResourcesRecovery;
                p.ResourcesRecoveryPercentages = double.Round(resourcesRecoveryPercentages * 100, 2);

                return p;
            })
            .OrderByDescending(p => p.DamageDone)];

        GetCombatAverageInformation(Combat.Duration, Players);

        DamageDoneScoreVM.Prepare(Players);
        HealDoneScoreVM.Prepare(Players);
        DamageTakenScoreVM.Prepare(Players);
        ResourcesRecoveryScoreVM.Prepare(Players);
        PlayerInfoVM.Prepare(Players);

        base.Prepare();
    }

    private void OrderBy(int tabindex)
    {
        switch (tabindex)
        {
            case 1:
                DamageDoneScoreVM.OrderBy(tabindex);
                break;
            case 2:
                HealDoneScoreVM.OrderBy(tabindex);
                break;
            case 3:
                DamageTakenScoreVM.OrderBy(tabindex);
                break;
            case 4:
                ResourcesRecoveryScoreVM.OrderBy(tabindex);
                break;
            default:
                break;
        }
    }

    private static void GetCombatAverageInformation(string durationStr, List<CombatPlayerModel> players)
    {
        if (TimeSpan.TryParse(durationStr, out var duration))
        {
            foreach (var player in players)
            {
                player.DamageDonePerSecond = player.DamageDone / duration.TotalSeconds;
                player.HealDonePerSecond = player.HealDone / duration.TotalSeconds;
                player.ResourcesRecoveryPerSecond = player.ResourcesRecovery / duration.TotalSeconds;
                player.DamageTakenPerSecond = player.DamageTaken / duration.TotalSeconds;
            }
        }
    }
}