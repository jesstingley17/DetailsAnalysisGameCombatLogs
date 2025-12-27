using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class BossRepository(CombatParserContext context) : IBossRepository 
{
    private readonly CombatParserContext _context = context;

    public async Task<Boss?> GetAsync(int gameBossId, int difficult, int groupSize)
    {
        var boss = await _context.Set<Boss>()
                     .SingleOrDefaultAsync(b => b.GameId == gameBossId && b.Difficult == difficult && b.Size == groupSize);

        return boss;
    }
}
