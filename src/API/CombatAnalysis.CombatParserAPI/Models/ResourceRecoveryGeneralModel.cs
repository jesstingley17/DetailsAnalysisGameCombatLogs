using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class ResourceRecoveryGeneralModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Spell { get; set; }

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public double ResourcePerSecond { get; set; }

    [Range(0, int.MaxValue)]
    public int CastNumber { get; set; }

    [Range(0, int.MaxValue)]
    public int MinValue { get; set; }

    [Range(0, int.MaxValue)]
    public int MaxValue { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageValue { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
