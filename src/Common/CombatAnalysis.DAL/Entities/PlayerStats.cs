using CombatAnalysis.DAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.DAL.Entities;

public class PlayerStats : IEntity
{
    public int Id { get; set; }

    public int Faction { get; set; }

    public int Strength { get; set; }

    public int Agility { get; set; }

    public int Intelligence { get; set; }

    public int Stamina { get; set; }

    public int Spirit { get; set; }

    public int Dodge { get; set; }

    public int Parry { get; set; }

    public int Crit { get; set; }

    public int Haste { get; set; }

    public int Hit { get; set; }

    public int Expertise { get; set; }

    public int Armor { get; set; }

    [MaxLength(126)]
    public string Talents { get; set; } = string.Empty;

    public int CombatPlayerId { get; set; }
}
