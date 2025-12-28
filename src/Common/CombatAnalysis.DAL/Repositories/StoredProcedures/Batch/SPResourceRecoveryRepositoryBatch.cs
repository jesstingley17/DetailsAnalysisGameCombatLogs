using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPResourceRecoveryRepositoryBatch(CombatParserContext context) : SPGenericRepository<ResourceRecovery>(context), IGenericRepositoryBatch<ResourceRecovery>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<ResourceRecovery> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(ResourceRecovery.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(ResourceRecovery.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(ResourceRecovery.Time), firstElement.Time.GetType());
        table.Columns.Add(nameof(ResourceRecovery.Creator), firstElement.Creator.GetType());
        table.Columns.Add(nameof(ResourceRecovery.Target), firstElement.Target.GetType());
        table.Columns.Add(nameof(ResourceRecovery.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Spell,
                item.Value,
                item.Time,
                item.Creator,
                item.Target,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(ResourceRecovery)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(ResourceRecovery)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
