using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class SpecScoreRepository(CombatParserSQLContext context) : ISpecScore
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<IEnumerable<SpecializationScore>> GetBySpecIdAsync(int specId, int bossId, int difficult)
    {
        var procName = $"Get{typeof(SpecializationScore).Name}BySpecId";
        var data = await Task.Run(() => _context.Set<SpecializationScore>()
                            .FromSql($"{procName} @specId={specId}, @bossId={bossId}, @difficult={difficult}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }
}
