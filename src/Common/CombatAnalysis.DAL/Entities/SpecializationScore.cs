using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class SpecializationScore : IEntity
{
    public int Id { get; set; }

    public double DamageScore { get; set; }

    public int DamageDone { get; set; }

    public double HealScore { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public Specialization Specialization { get; set; }

    public int SpecializationId { get; set; }

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
