using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures;

internal class SPDamageTakenRepositoryBatch(CombatParserContext context) : SPGenericRepository<DamageTaken>(context), IGenericRepositoryBatch<DamageTaken>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageTaken> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(DamageTaken.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(DamageTaken.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(DamageTaken.Time), firstElement.Time.GetType());
        table.Columns.Add(nameof(DamageTaken.Creator), firstElement.Creator.GetType());
        table.Columns.Add(nameof(DamageTaken.Target), firstElement.Target.GetType());
        table.Columns.Add(nameof(DamageTaken.DamageTakenType), firstElement.DamageTakenType.GetType());
        table.Columns.Add(nameof(DamageTaken.ActualValue), firstElement.ActualValue.GetType());
        table.Columns.Add(nameof(DamageTaken.IsPeriodicDamage), firstElement.IsPeriodicDamage.GetType());
        table.Columns.Add(nameof(DamageTaken.Resisted), firstElement.Resisted.GetType());
        table.Columns.Add(nameof(DamageTaken.Absorbed), firstElement.Absorbed.GetType());
        table.Columns.Add(nameof(DamageTaken.Blocked), firstElement.Blocked.GetType());
        table.Columns.Add(nameof(DamageTaken.RealDamage), firstElement.RealDamage.GetType());
        table.Columns.Add(nameof(DamageTaken.Mitigated), firstElement.Mitigated.GetType());
        table.Columns.Add(nameof(DamageTaken.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Spell,
                item.Value,
                item.Time,
                item.Creator,
                item.Target,
                item.DamageTakenType,
                item.ActualValue,
                item.IsPeriodicDamage,
                item.Resisted,
                item.Absorbed,
                item.Blocked,
                item.RealDamage,
                item.Mitigated,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageTaken)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(DamageTaken)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
