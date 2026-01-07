using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class PlayerInfoPaginationRepository<TModel>(CombatParserContext context) : PlayerInfoRepository<TModel>(context), IPlayerInfoPaginationRepository<TModel>
    where TModel : class, IEntity, ICombatPlayerEntity, ITimeEntity
{
    private readonly CombatParserContext _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize)
    {
        var data = await _context.Set<TModel>()
                            .AsNoTracking()
                            .Where(x => x.CombatPlayerId == combatPlayerId)
                            .OrderBy(p => p.Time)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }
}