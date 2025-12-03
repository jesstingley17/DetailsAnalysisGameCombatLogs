using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatLogModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public DateTimeOffset Date { get; set; }

    [Range(0, int.MaxValue)]
    public int LogType { get; set; }

    [Range(0, int.MaxValue)]
    public int NumberReadyCombats { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatsInQueue { get; set; }

    [Required]
    public bool IsReady { get; set; }

    [Required]
    public string AppUserId { get; set; }
}
