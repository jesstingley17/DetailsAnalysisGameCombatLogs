using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class Boss : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }
}
