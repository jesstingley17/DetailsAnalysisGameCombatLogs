namespace CombatAnalysis.DAL.Interfaces.Entities;

/// <summary>
/// Each entity, which has refer to CombatPlayerId, should implement this contract
/// </summary>
public interface ICombatPlayerEntity : IEntity
{
    /// <summary>
    /// Refer to CombatPlayer Id
    /// </summary>
    int CombatPlayerId { get; set; }
}