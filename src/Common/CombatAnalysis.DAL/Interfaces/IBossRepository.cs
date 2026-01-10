using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IBossRepository
{
    Task<Boss?> GetById(int bossId, CancellationToken cancellationToken);

    Task<Boss?> GetAsync(int gameBossId, int difficult, int groupSize, CancellationToken cancellationToken);
}
