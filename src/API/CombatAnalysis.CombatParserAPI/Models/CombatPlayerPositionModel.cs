using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerPositionModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public double PositionX { get; set; }

    [Range(0, int.MaxValue)]
    public double PositionY { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
