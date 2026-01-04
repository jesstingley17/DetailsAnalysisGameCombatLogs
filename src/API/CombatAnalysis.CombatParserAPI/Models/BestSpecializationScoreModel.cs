namespace CombatAnalysis.CombatParserAPI.Models;

public class BestSpecializationScoreModel
{
    public int Id { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public int SpecializationId { get; set; }
}
