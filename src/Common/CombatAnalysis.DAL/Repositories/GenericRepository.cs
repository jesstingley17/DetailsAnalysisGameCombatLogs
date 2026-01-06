using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class GenericRepository<TModel>(CombatParserContext context) : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserContext _context = context;

    public async Task<TModel?> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        var existing = await _context.Set<TModel>().FindAsync(item.Id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Set<TModel>().Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var result = await _context.Set<TModel>().AsNoTracking().ToListAsync();
        return result.Count != 0 ? result : [];
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value)
    {
        var data = await _context.Set<TModel>()
                    .Where(x => EF.Property<object>(x, paramName).Equals(value))
                    .AsNoTracking()
                    .ToListAsync();

        return data.Count != 0 ? data : [];
    }
}
