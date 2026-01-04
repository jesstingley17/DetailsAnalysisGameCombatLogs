using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class BestSpecializationScore
{
    public int Id { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public int SpecializationId { get; set; }

    public int BossId { get; set; }
}
