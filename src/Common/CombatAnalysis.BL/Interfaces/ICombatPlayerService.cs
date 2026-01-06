using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface ICombatPlayerService
{
    Task<CombatPlayerDto> CreateAsync(CombatPlayerDto item);

    Task<int> UpdateAsync(int id, CombatPlayerDto item);

    Task<IEnumerable<CombatPlayerDto>> GetByCombatIdAsync(int combatId);
}
