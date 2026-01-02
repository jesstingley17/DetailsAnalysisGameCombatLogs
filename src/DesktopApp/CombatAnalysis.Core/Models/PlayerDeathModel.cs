namespace CombatAnalysis.Core.Models;

public class PlayerDeathModel
{
    public string Username { get; set; } = string.Empty;

    public string LastHitSpell { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
