using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class GenericRepository<TModel>(CombatParserSQLContext context) : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<TModel?> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        var existing = await _context.Set<TModel>().FindAsync(item.Id);

        if (existing != null)
        {
            _context.Entry(existing).State = EntityState.Detached;
        }

        _context.Set<TModel>().Update(item);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity == null)
        {
            return 0;
        }

        _context.Set<TModel>().Remove(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var result = await _context.Set<TModel>().AsNoTracking().ToListAsync();
        return result;
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
        var query = _context.Set<TModel>().AsNoTracking();

        var filteredQuery = query.Where(x => EF.Property<object>(x, paramName).Equals(value));
        var any = await filteredQuery.AnyAsync();

        var result = any ? filteredQuery.AsEnumerable() : [];

        return result;
    }
}
