using CombatAnalysis.DAL.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.DAL.Entities.CombatPlayerData;

public class ResourceRecovery : ICombatPlayerEntity, IGeneralFilterEntity
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    [MaxLength(126)]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    [MaxLength(126)]
    public string Creator { get; set; } = string.Empty;

    [MaxLength(126)]
    public string Target { get; set; } = string.Empty;

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
