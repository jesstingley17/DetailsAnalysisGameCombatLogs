using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class PlayerDeathModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string LastHitSpell { get; set; }

    [Range(0, int.MaxValue)]
    public int LastHitValue { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
