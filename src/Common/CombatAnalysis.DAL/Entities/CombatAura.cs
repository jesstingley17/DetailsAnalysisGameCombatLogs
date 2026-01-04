using CombatAnalysis.DAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.DAL.Entities;

public class CombatAura : IEntity
{
    public int Id { get; set; }

    [MaxLength(126)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(126)]
    public string Creator { get; set; } = string.Empty;

    [MaxLength(126)]
    public string Target { get; set; } = string.Empty;

    public int AuraCreatorType { get; set; }

    public int AuraType { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public int Stacks { get; set; }

    public int CombatId { get; set; }
}
