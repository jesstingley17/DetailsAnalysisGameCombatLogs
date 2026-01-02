using CombatAnalysis.BL.Interfaces.Entity;

namespace CombatAnalysis.BL.DTO;

public class PlayerDeathDto : ICombatPlayerEntity
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string LastHitSpell { get; set; }

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
