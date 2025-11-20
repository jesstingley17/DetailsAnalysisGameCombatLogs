using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.StoredProcedure;

internal class SPGenericRepository<TModel>(CombatParserSQLContext context) : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<TModel?> CreateAsync(TModel item)
    {
        var type = item.GetType();
        var properties = type.GetProperties();

        var procName = $"InsertInto{type.Name}";
        var parameters = new List<SqlParameter>();

        for (var i = 1; i < properties.Length; i++)
        {
            if (!properties[i].CanWrite) continue;

            var value = properties[i].GetValue(item);

            parameters.Add(new SqlParameter($"@{properties[i].Name}", value ?? DBNull.Value));
        }

        var paramPlaceholders = string.Join(",", parameters.Select(p => p.ParameterName));

#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
        var data = await _context.Set<TModel>()
                        .FromSqlRaw($"{procName} {paramPlaceholders}", [.. parameters])
                        .ToListAsync();
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.

        return data.FirstOrDefault();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var procName = $"Delete{typeof(TModel).Name}ById";
        var rowsAffected = await _context.Database
                            .ExecuteSqlAsync($"{procName} @id={id}");

        return rowsAffected;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var procName = $"GetAll{typeof(TModel).Name}";
        var data = await _context.Set<TModel>()
                            .FromSqlRaw(procName)
                            .ToListAsync();

        return data.Count != 0 ? data : [];
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        var procName = $"Get{typeof(TModel).Name}ById";
        var data = await _context.Set<TModel>()
                            .FromSql($"{procName} @id={id}")
                            .FirstOrDefaultAsync();

        return data;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value)
    {
        var data = await _context.Set<TModel>()
                    .Where(x => EF.Property<object>(x, paramName).Equals(value))
                    .ToListAsync();

        return data.Count != 0 ? data : [];
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        var type = item.GetType();
        var properties = type.GetProperties();

        var procName = $"Update{type.Name}";
        var parameters = new List<SqlParameter>();

        foreach (var prop in properties)
        {
            if (!prop.CanWrite) continue;

            var value = prop.GetValue(item);

            parameters.Add(new SqlParameter($"@{prop.Name}", value ?? DBNull.Value));
        }

        var paramPlaceholders = string.Join(",", parameters.Select(p => p.ParameterName));

#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"{procName} {paramPlaceholders}", [.. parameters]);
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.

        return rowsAffected;
    }
}
