using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class PlayerRepository(CombatParserContext context) : IPlayerRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<Player?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var player = await _context.Set<Player>()
             .FindAsync(id, cancellationToken);

        return player;
    }

    public async Task<Player?> GetByGameIdAsync(string gameId, CancellationToken cancellationToken)
    {
        var player = await _context.Set<Player>()
             .SingleOrDefaultAsync(b => b.GameId == gameId, cancellationToken);

        return player;
    }

    public async Task<Player> CreateAsync(Player player, CancellationToken cancellationToken)
    {
        var entityEntry = await _context.Set<Player>().AddAsync(player, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity;
    }
}

