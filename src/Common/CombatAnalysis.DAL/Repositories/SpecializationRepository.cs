using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class SpecializationRepository(CombatParserContext context) : ISpecializationRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<Specialization?> GetBySpellsAsync(string spells, CancellationToken cancellationToken)
    {
        var inputIds = spells
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToArray();

        var data = await _context.Set<Specialization>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s =>
                inputIds.Any(id =>
                    ("," + s.SpecializationSpellsId + ",")
                        .Contains("," + id + ",")
                ),
                cancellationToken);

        return data;
    }
}
