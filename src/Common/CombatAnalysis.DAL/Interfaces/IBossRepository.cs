using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IBossRepository
{
    Task<Boss?> GetAsync(int gameBossId, int difficult, int groupSize);
}
