using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public class PlayerInfoViewModel : ParentTemplate<List<CombatPlayerModel>>
{
    private List<CombatPlayerModel>? _players;
    private CombatPlayerModel? _selectedPlayer;

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
        }
    }

    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        Players = parameter;
    }
}
