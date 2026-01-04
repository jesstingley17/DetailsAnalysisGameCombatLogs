using CombatAnalysis.DAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.DAL.Entities;

public class Boss : IEntity
{
    public int Id { get; set; }

    public int GameId { get; set; }

    [MaxLength(126)]
    public string Name { get; set; } = string.Empty;

    public long Health { get; set; }

    public int Difficult { get; set; }

    public int Size { get; set; }
}
