using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.Filters;

internal class GeneralFilterRepositroy<TModel>(CombatParserSQLContext context) : IGeneralFilterRepository<TModel>
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

    public async Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
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

    public async Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize)
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

    public async Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize)
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
