using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class PlayerInfoRepository<TModel>(CombatParserContext context) : IPlayerInfoRepository<TModel>
    where TModel : class, IEntity, ICombatPlayerEntity
{
    private readonly CombatParserContext _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
                            .AsNoTracking()
                            .Where(x => x.CombatPlayerId == combatPlayerId)
                            .ToListAsync(cancellationToken);

        return data.Count != 0 ? data : [];
    }
}
