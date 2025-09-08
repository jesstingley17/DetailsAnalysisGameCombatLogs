using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL;

internal class SQLRepository<TModel, TIdType>(CommunicationSQLContext context) : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    private readonly CommunicationSQLContext _context = context;

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
