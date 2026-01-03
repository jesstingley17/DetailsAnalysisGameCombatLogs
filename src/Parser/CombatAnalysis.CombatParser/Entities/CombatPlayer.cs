namespace CombatAnalysis.CombatParser.Entities;

public class CombatPlayer
{
    public double AverageItemLevel { get; set; }

    public int DamageDoneToBoss { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public PlayerStats Stats { get; set; } = new();

    public PlayerParseInfo PlayerParseInfo { get; set; } = new();

    public Player Player { get; set; } = new();

    public int CombatId { get; set; }
}
