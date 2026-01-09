using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class SpecializationScoreRepository(CombatParserContext context) : ISpecializationScoreRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<int> UpdateAsync(SpecializationScore item, CancellationToken cancellationToken)
    {
        var existing = await _context.Set<SpecializationScore>().FindAsync(item.Id, cancellationToken) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SpecializationScore?> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<SpecializationScore>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CombatPlayerId == combatPlayerId, cancellationToken);

        return data;
    }
}
