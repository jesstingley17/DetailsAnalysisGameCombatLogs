using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public class ResourcesRecoveryScoreViewModel : BasicCombatPlayerViewModel
{
    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        Players = parameter;
        _defaultPlayers = parameter;

        BestValue = Players.Max(p => p.ResourcesRecovery);

        var value = Players.Average(x => x.ResourcesRecovery);
        AverageValue = double.Round(value, 2);
        AverageVPS = Players.Average(x => x.ResourcesRecoveryPerSecond);

        TotalValue = Players.Sum(x => x.ResourcesRecovery);
        TotalVPS = Players.Sum(x => x.ResourcesRecoveryPerSecond);

        ValueType = 3;

        base.Prepare();
    }

    public override void OrderBy(int tabIndex)
    {
        if (Players == null)
        {
            return;
        }

        Players = [.. Players.OrderByDescending(p => p.ResourcesRecovery)];
    }
}
