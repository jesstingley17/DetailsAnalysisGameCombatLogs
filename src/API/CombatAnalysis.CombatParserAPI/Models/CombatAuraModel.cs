using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatAuraModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Creator { get; set; }

    [Required]
    public string Target { get; set; }

    [Range(0, int.MaxValue)]
    public int AuraCreatorType { get; set; }

    [Range(0, int.MaxValue)]
    public int AuraType { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }

    [Range(0, int.MaxValue)]
    public int Stacks { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
