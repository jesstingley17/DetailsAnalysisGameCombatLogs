using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task<Player?> GetByGameIdAsync(string gameId, CancellationToken cancellationToken);

    Task<Player> CreateAsync(Player player, CancellationToken cancellationToken);
}
