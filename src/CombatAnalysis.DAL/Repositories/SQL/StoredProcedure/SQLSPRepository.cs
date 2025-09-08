using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLSPRepository<TModel>(CombatParserSQLContext context) : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context = context;

    public async Task<TModel> CreateAsync(TModel item)
    {
        var properties = item.GetType().GetProperties();
        var procedureParamNames = new StringBuilder();

        for (var i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParamNames.Append($"@{properties[i].Name}={properties[i].GetValue(item)},");
            }
        }

        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        var procName = $"InsertInto{item.GetType().Name}";
        var data = await Task.Run(() => _context.Set<TModel>().FromSql($"{procName} {procedureParamNames}")
                                            .AsEnumerable()
                                            .FirstOrDefault());

        return data;
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
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSql($"{procName}")
                            .AsEnumerable());

        return data.Any() ? data : [];
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        var procName = $"Get{typeof(TModel).Name}ById";
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSql($"{procName} @id={id}")
                            .AsEnumerable()
                            .FirstOrDefault());

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
        var properties = item.GetType().GetProperties();
        var procedureParamNames = new StringBuilder();
        for (var i = 0; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParamNames.Append($"@{properties[i].Name}={properties[i].GetValue(item)},");
            }
        }

        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        var procName = $"Update{item.GetType().Name}";
        var rowsAffected = await _context.Database
                            .ExecuteSqlAsync($"{procName} {procedureParamNames}");

        return rowsAffected;
    }
}
