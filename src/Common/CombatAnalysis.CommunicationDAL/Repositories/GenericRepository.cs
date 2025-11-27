using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationDAL.Repositories;

internal class GenericRepository<TModel, TIdType>(CommunicationContext context) : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    protected readonly CommunicationContext _context = context;

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> UpdateAsync(TIdType id, TModel item)
    {
        var existing = await _context.Set<TModel>().FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(TIdType id)
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

    public async Task<TModel?> GetByIdAsync(TIdType id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync<TValue>(Expression<Func<TModel, TValue>> property, TValue value)
    {
        var parameter = Expression.Parameter(typeof(TModel), "param");

        var body = Expression.Equal(
            Expression.Invoke(property, parameter),
            Expression.Constant(value, typeof(TValue))
        );

        var lambda = Expression.Lambda<Func<TModel, bool>>(body, parameter);

        var query = await _context.Set<TModel>()
                                .AsNoTracking()
                                .Where(lambda)
                                .ToListAsync();

        return query;
    }
}
