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
    public PlayerStatsModel Stats { get; set; }

    [Required]
    public PlayerModel Player { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
