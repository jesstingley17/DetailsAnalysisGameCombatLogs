using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL;

internal class SQLRepository<TModel, TIdType>(CommunicationSQLContext context) : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    protected readonly CommunicationSQLContext _context = context;

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(TIdType id)
    {
        var model = Activator.CreateInstance<TModel>();
        typeof(TModel).GetProperty("Id")?.SetValue(model, id);

        _context.Attach(model);
        _context.Set<TModel>().Remove(model);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
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

    public async Task<int> UpdateAsync(TModel item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
