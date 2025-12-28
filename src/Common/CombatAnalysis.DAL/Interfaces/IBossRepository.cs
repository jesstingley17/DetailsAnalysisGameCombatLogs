using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IBossRepository
{
    Task<Boss?> GetById(int bossId);

    Task<Boss?> GetAsync(int gameBossId, int difficult, int groupSize);
}
