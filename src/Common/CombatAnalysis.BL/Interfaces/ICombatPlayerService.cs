using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface ICombatPlayerService
{
    Task CreateBatchAsync(IEnumerable<CombatPlayerDto> items, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayerDto>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
