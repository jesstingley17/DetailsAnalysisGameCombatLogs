namespace CombatAnalysis.BL.DTO;

public class CombatPlayerDto
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public int ResourcesRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public CombatPlayerStatsDto Stats { get; set; } = new();

    public SpecializationScoreDto Score { get; set; } = new();

    public PlayerDto Player { get; set; } = new();

    public string PlayerId { get; set; }

    public int CombatId { get; set; }
}
