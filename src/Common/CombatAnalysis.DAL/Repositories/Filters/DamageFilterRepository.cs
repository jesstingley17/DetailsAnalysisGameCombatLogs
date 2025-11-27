using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.Filters;

internal class DamageFilterRepository(CombatParserContext context) : IDamageFilterRepository
{
    private readonly CombatParserContext _context = context;

    public async Task<IEnumerable<List<CombatTarget>>> GetDamageByEachTargetAsync(int combatId)
    {
        var damageByEachTarget = new List<List<CombatTarget>>();
        var targets = await GetTargetsAsync(combatId);

        foreach (var item in targets)
        {
            var sum = await _context.Set<Combat>()
                       .Where(x => x.Id == combatId)
                       .Join(_context.Set<CombatPlayer>(),
                           x => x.Id,
                           u => u.CombatId,
                           (x, u) => new
                           {
                               u.Id,
                               u.Username
                           })
                       .Join(_context.Set<DamageDone>(),
                           x => x.Id,
                           u => u.CombatPlayerId,
                           (x, u) => new
                           {
                               x.Username,
                               u.Target,
                               u.Value
                           })
                       .Where(x => x.Target == item)
                       .GroupBy(x => x.Username)
                       .Select(x => new CombatTarget { Username = x.Key, Target = item, Sum = x.Sum(y => y.Value) })
                       .OrderByDescending(x => x.Sum)
                       .ToListAsync();

            damageByEachTarget.Add(sum);
        }

        return damageByEachTarget;
    }

    private async Task<List<string>> GetTargetsAsync(int combatId)
    {
        var targets = await _context.Set<Combat>()
                .Where(x => x.Id == combatId)
                .Join(_context.Set<CombatPlayer>(),
                    x => x.Id,
                    u => u.CombatId,
                    (x, u) => new
                    {
                        u.Id,
                    })
                .Join(_context.Set<DamageDone>(),
                    x => x.Id,
                    u => u.CombatPlayerId,
                    (x, u) => new
                    {
                        u.Target
                    })
                .Distinct()
                .Select(x => x.Target)
                .ToListAsync();

        return targets;
    }
}
