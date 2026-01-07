using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class SpecializationScoreRepository(CombatParserContext context) : ISpecializationScoreRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<int> UpdateAsync(SpecializationScore item)
    {
        var existing = await _context.Set<SpecializationScore>().FindAsync(item.Id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<SpecializationScore>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Set<SpecializationScore>().Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<SpecializationScore?> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var data = await _context.Set<SpecializationScore>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CombatPlayerId == combatPlayerId);

        return data;
    }
}
