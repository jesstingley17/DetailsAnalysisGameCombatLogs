namespace CombatAnalysis.Core.Models.GameLogs;

public class SpecializationScoreModel
{
    public double DamageScore { get; set; }

    public double HealScore { get; set; }

    public DateTimeOffset Updated { get; set; }

    public int SpecializationId { get; set; }

    public int CombatPlayerId { get; set; }
}
