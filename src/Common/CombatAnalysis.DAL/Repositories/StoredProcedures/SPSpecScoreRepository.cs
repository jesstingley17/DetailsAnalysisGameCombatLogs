using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures;

internal class SPSpecScoreRepository(CombatParserContext context) : ISpecScore
{
    private readonly CombatParserContext _context = context;

    public async Task<IEnumerable<SpecializationScore>> GetBySpecIdAsync(int specId, int bossId)
    {
        var procName = $"Get{typeof(SpecializationScore).Name}BySpecId";
        var data = await _context.Set<SpecializationScore>()
                            .FromSql($"{procName} @specId={specId}, @bossId={bossId}")
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }
}
