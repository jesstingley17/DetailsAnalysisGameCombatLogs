using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CombatAnalysis.UserDAL.Repositories;

internal class GenericRepository<TModel, TIdType>(UserContext context) : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    private readonly UserContext _context = context;

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(TIdType id)
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
        var collection = await _context.Set<TModel>().AsNoTracking().ToListAsync();
        return collection;
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

    public async Task<int> UpdateAsync(TIdType id, TModel item)
    {
        var existing = await _context.Set<TModel>().FindAsync(id);

        if (existing != null)
        {
            _context.Entry(existing).State = EntityState.Detached;
        }

        _context.Set<TModel>().Update(item);
        return await _context.SaveChangesAsync();
    }
}
