namespace CombatAnalysis.EnhancedWebApp.Server.Models;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public int ResourcesRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public PlayerModel Player { get; set; }

    public int CombatId { get; set; }
}
