using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class HealDoneModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Spell { get; set; }

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public int Overheal { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string Creator { get; set; }

    [Required]
    public string Target { get; set; }

    [Required]
    public bool IsCrit { get; set; }

    [Required]
    public bool IsAbsorbed { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
