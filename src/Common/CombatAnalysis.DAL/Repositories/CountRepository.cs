using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class CountRepository<TModel>(CombatParserContext context) : ICountRepository<TModel>
    where TModel : class, ICombatPlayerEntity
{
    private readonly CombatParserContext _context = context;

    public async Task<int> CountByCombatPlayerIdAsync(int combatPlayerId)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId);

        return count;
    }
}
