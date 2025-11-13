using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLSPPlayerInfoRepository<TModel>(CombatParserSQLContext context) : IPlayerInfoRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var procName = $"Get{typeof(TModel).Name}ByCombatPlayerId";
        var data = await _context.Set<TModel>()
                            .FromSql($"{procName} @combatPlayerId={combatPlayerId}")
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize)
    {
        var procName = $"Get{typeof(TModel).Name}ByCombatPlayerIdPagination";
        var data = await _context.Set<TModel>()
                            .FromSql($"{procName} @combatPlayerId={combatPlayerId}, @page={page}, @pageSize={pageSize}")
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }
}