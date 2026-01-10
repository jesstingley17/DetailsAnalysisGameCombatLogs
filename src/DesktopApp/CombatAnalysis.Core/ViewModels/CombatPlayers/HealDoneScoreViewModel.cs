using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public class HealDoneScoreViewModel : BasicCombatPlayerViewModel
{
    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        Players = parameter;
        _defaultPlayers = parameter;

        BestValue = Players.Max(p => p.HealDone);

        var value = Players.Average(x => x.HealDone);
        AverageValue = double.Round(value, 2);
        AverageVPS = Players.Average(x => x.HealDonePerSecond);

        TotalValue = Players.Sum(x => x.HealDone);
        TotalVPS = Players.Sum(x => x.HealDonePerSecond);

        ValueType = 1;

        base.Prepare();
    }

    public override void OrderBy(int tabIndex)
    {
        if (Players == null)
        {
            return;
        }

        Players = [.. Players.OrderByDescending(p => p.HealDone)];
    }
}
