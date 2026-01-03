using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(string id);

    Task<Player?> GetByGameIdAsync(string gameId);

    Task<Player> CreateAsync(Player player);
}
