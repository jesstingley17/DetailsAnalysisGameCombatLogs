using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int LocallyNumber { get; set; }

    [Required]
    public string DungeonName { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0, int.MaxValue)]
    public int Difficulty { get; set; }

    [Required]
    public List<string> Data { get; set; }

    [Range(0, int.MaxValue)]
    public int EnergyRecovery { get; set; }

    [Range(0, int.MaxValue)]
    public long DamageDone { get; set; }

    [Range(0, int.MaxValue)]
    public long HealDone { get; set; }

    [Range(0, int.MaxValue)]
    public long DamageTaken { get; set; }

    [Required]
    public bool IsWin { get; set; }

    [Required]
    public DateTimeOffset StartDate { get; set; }

    [Required]
    public DateTimeOffset FinishDate { get; set; }

    [Required]
    public List<CombatPlayerModel> Players { get; set; }

    [Required]
    public Dictionary<string, List<string>> PetsId { get; set; }

    [Required]
    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    [Required]
    public bool IsReady { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatLogId { get; set; }
}
