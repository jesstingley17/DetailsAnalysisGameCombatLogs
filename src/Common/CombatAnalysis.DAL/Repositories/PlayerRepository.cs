using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class PlayerRepository(CombatParserContext context) : IPlayerRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<Player?> GetByIdAsync(string id)
    {
        var player = await _context.Set<Player>()
             .FindAsync(id);

        return player;
    }

    public async Task<Player?> GetByGameIdAsync(string gameId)
    {
        var player = await _context.Set<Player>()
             .SingleOrDefaultAsync(b => b.GameId == gameId);

        return player;
    }

    public async Task<Player> CreateAsync(Player player)
    {
        var entityEntry = await _context.Set<Player>().AddAsync(player);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }
}

