using CombatAnalysis.DAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.DAL.Entities;

public class CombatLog : IEntity
{
    public int Id { get; set; }

    [MaxLength(126)]
    public string Name { get; set; } = string.Empty;

    public DateTimeOffset Date { get; set; }

    public int LogType { get; set; }

    public int NumberReadyCombats { get; set; }

    public int CombatsInQueue { get; set; }

    public bool IsReady { get; set; }

    public string AppUserId { get; set; } = string.Empty;

    public ICollection<Combat> Combats { get; set; } = [];
}