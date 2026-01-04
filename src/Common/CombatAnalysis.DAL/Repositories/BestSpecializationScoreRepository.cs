using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class BestSpecializationScoreRepository(CombatParserContext context) : IBestSpecializationScoreRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<int> UpdateAsync(int id, BestSpecializationScore item)
    {
        var entity = await _context.Set<BestSpecializationScore>()
            .FindAsync(id);

        if (entity == null)
        {
            return 0;
        }

        _context.Entry(entity).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<BestSpecializationScore?> GetAsync(int specializationId, int bossId)
    {
        var entity = await _context.Set<BestSpecializationScore>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SpecializationId == specializationId && x.BossId == bossId);

        return entity;
    }
}
