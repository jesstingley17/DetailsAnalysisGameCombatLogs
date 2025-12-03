using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class DamageDoneModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Spell { get; set; }

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string Creator { get; set; }

    [Required]
    public string Target { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageType { get; set; }

    [Required]
    public bool IsPeriodicDamage { get; set; }

    [Required]
    public bool IsPet { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
