using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

internal class SQLRepository<TModel>(CombatParserSQLContext context) : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var item = Activator.CreateInstance<TModel>();
        typeof(TModel).GetProperty("Id")?.SetValue(item, id);

        _context.Attach(item);
        _context.Set<TModel>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
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
        var query = _context.Set<TModel>().AsNoTracking();

        var filteredQuery = query.Where(x => EF.Property<object>(x, paramName).Equals(value));
        var any = await filteredQuery.AnyAsync();

        var result = any ? filteredQuery.AsEnumerable() : [];

        return result;
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
