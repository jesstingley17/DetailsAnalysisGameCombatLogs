using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.Filters;

internal class GeneralFilterRepositroy<TModel>(CombatParserSQLContext context) : IGeneralFilter<TModel>
    where TModel : class, IGeneralFilterEntity, ICombatPlayerEntity
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Target)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync();

        return uniqueValues;
    }

    public async Task<int> CountTargetByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target));

        return count;
    }

    public async Task<IEnumerable<TModel>> GetTargetByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
    }

    public async Task<IEnumerable<List<CombatTarget>>> GetDamageByEachTargetAsync(int combatId)
    {
        var damageByEachTarget = new List<List<CombatTarget>>();
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
                        .ToListAsync();

            damageByEachTarget.Add(sum);
        }

        return damageByEachTarget;
    }

    public async Task<int> GetTargetValueByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .SumAsync(x => x.Value);

        return values;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Creator)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync();

        return uniqueValues;
    }

    public async Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator));

        return count;
    }

    public async Task<IEnumerable<TModel>> GetCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
    }

    public async Task<IEnumerable<string>> GetSpellNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync();

        return uniqueValues;
    }

    public async Task<int> CountSpellByCombatPlayerIdAsync(int combatPlayerId, string spell)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Spell.Equals(spell));

        return count;
    }

    public async Task<IEnumerable<TModel>> GetSpellByCombatPlayerIdAsync(int combatPlayerId, string spell, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Spell.Equals(spell))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
    }
}
