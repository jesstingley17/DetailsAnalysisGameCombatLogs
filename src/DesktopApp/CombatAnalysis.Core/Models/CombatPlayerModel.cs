namespace CombatAnalysis.Core.Models;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PlayerId { get; set; } = string.Empty;

    public double AverageItemLevel { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public double DamageDonePerSecond { get; set; }

    public double HealDonePerSecond { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public double ResourcesRecoveryPerSecond { get; set; }

    public PlayerStatsModel Stats { get; set; } = new();

    public PlayerParseInfoModel PlayerParseInfo { get; set; } = new();

    public double DamageDonePercentages { get; set; }

    public double HealDonePercentages { get; set; }

    public double DamageTakenPercentages { get; set; }

    public double ResourcesRecoveryPercentages { get; set; }

    public int CombatId { get; set; }
}
