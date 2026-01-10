using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class GenericRepository<TModel>(CombatParserContext context) : ICreateEntityRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserContext _context = context;

    public async Task<TModel?> CreateAsync(TModel item, CancellationToken cancelationToken)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item, cancelationToken);
        await _context.SaveChangesAsync(cancelationToken);

        return entityEntry.Entity;
    }

    public async Task<int> UpdateAsync(TModel item, CancellationToken cancelationToken)
    {
        var existing = await _context.Set<TModel>().FindAsync(item.Id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancelationToken)
    {
        var entity = await _context.Set<TModel>().FindAsync(id, cancelationToken);
        if (entity == null)
        {
            return false;
        }

        _context.Set<TModel>().Remove(entity);
        await _context.SaveChangesAsync(cancelationToken);

        return true;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancelationToken)
    {
        var result = await _context.Set<TModel>()
            .AsNoTracking()
            .ToListAsync(cancelationToken);
        return result.Count != 0 ? result : [];
    }

    public async Task<TModel?> GetByIdAsync(int id, CancellationToken cancelationToken)
    {
        var entity = await _context.Set<TModel>().FindAsync(id, cancelationToken);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value, CancellationToken cancelationToken)
    {
        var data = await _context.Set<TModel>()
                    .Where(x => EF.Property<object>(x, paramName).Equals(value))
                    .AsNoTracking()
                    .ToListAsync(cancelationToken);

        return data.Count != 0 ? data : [];
    }
}
