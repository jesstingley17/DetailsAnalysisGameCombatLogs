using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPCombatAuraRepositoryBatch(CombatParserContext context) : SPGenericRepository<CombatAura>(context), IGenericRepositoryBatch<CombatAura>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<CombatAura> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(CombatAura.Name), firstElement.Name.GetType());
        table.Columns.Add(nameof(CombatAura.Creator), firstElement.Creator.GetType());
        table.Columns.Add(nameof(CombatAura.Target), firstElement.Target.GetType());
        table.Columns.Add(nameof(CombatAura.AuraCreatorType), firstElement.AuraCreatorType.GetType());
        table.Columns.Add(nameof(CombatAura.AuraType), firstElement.AuraType.GetType());
        table.Columns.Add(nameof(CombatAura.StartTime), firstElement.StartTime.GetType());
        table.Columns.Add(nameof(CombatAura.FinishTime), firstElement.FinishTime.GetType());
        table.Columns.Add(nameof(CombatAura.Stacks), firstElement.Stacks.GetType());
        table.Columns.Add(nameof(CombatAura.CombatId), firstElement.CombatId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Name,
                item.Creator,
                item.Target,
                item.AuraCreatorType,
                item.AuraType,
                item.StartTime,
                item.FinishTime,
                item.Stacks,
                item.CombatId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(CombatAura)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(CombatAura)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
