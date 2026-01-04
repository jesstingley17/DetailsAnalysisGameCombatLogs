using CombatAnalysis.DAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.DAL.Entities;

public class PlayerDeath : ICombatPlayerEntity
{
    public int Id { get; set; }

    [MaxLength(126)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(126)]
    public string LastHitSpell { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
