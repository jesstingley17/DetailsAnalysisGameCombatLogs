using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface IBossService
{
    Task<BossDto?> GetById(int bossId);

    Task<BossDto?> GetAsync(int gameBossId, int difficult, int groupSize);
}
