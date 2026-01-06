using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageItemLevel { get; set; }

    [Range(0, int.MaxValue)]
    public int ResourcesRecovery { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageDone { get; set; }

    [Range(0, int.MaxValue)]
    public int HealDone { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageTaken { get; set; }

    [Required]
    public CombatPlayerStatsModel Stats { get; set; } = new();

    [Required]
    public SpecializationScoreModel Score { get; set; } = new();

    [Required]
    public PlayerModel Player { get; set; } = new();

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
