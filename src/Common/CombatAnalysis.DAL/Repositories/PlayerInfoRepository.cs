using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class PlayerInfoRepository<TModel>(CombatParserContext context) : IPlayerInfoRepository<TModel>
    where TModel : class, ICombatPlayerEntity
{
    private readonly CombatParserContext _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var data = await _context.Set<TModel>()
                            .AsNoTracking()
                            .Where(x => x.CombatPlayerId == combatPlayerId)
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize)
    {
        var data = await _context.Set<TModel>()
                            .AsNoTracking()
                            .Where(x => x.CombatPlayerId == combatPlayerId)
                            .Skip(page * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }
}