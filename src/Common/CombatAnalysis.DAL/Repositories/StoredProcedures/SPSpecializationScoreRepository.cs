using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures;

internal class SPSpecializationScoreRepository(CombatParserContext context) : ISpecializationScoreRepository
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<SpecializationScore> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(SpecializationScore.DamageScore), firstElement.DamageScore.GetType());
        table.Columns.Add(nameof(SpecializationScore.DamageDone), firstElement.DamageDone.GetType());
        table.Columns.Add(nameof(SpecializationScore.HealScore), firstElement.HealScore.GetType());
        table.Columns.Add(nameof(SpecializationScore.HealDone), firstElement.HealDone.GetType());
        table.Columns.Add(nameof(SpecializationScore.Updated), firstElement.Updated.GetValueOrDefault().GetType());
        table.Columns.Add(nameof(SpecializationScore.SpecializationId), firstElement.SpecializationId.GetType());
        table.Columns.Add(nameof(SpecializationScore.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.DamageScore,
                item.DamageDone,
                item.HealScore,
                item.HealDone,
                item.Updated,
                item.SpecializationId,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(SpecializationScore)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(SpecializationScore)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }

    public async Task<int> UpdateAsync(SpecializationScore item)
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

    public async Task<bool> DeleteAsync(int id)
    {
        var procName = $"Delete{nameof(SpecializationScore)}ById";
        var rowsAffected = await _context.Database
                            .ExecuteSqlAsync($"{procName} @id={id}");

        return rowsAffected > 0;
    }

    public async Task<SpecializationScore?> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var data = await _context.Set<SpecializationScore>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CombatPlayerId == combatPlayerId);

        return data;
    }
}
