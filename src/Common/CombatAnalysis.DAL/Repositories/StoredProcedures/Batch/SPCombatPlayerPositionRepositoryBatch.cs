using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPCombatPlayerPositionRepositoryBatch(CombatParserContext context) : SPGenericRepository<CombatPlayerPosition>(context), IGenericRepositoryBatch<CombatPlayerPosition>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<CombatPlayerPosition> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(CombatPlayerPosition.PositionX), firstElement.PositionX.GetType());
        table.Columns.Add(nameof(CombatPlayerPosition.PositionY), firstElement.PositionY.GetType());
        table.Columns.Add(nameof(CombatPlayerPosition.Time), firstElement.Time.GetType());
        table.Columns.Add(nameof(CombatPlayerPosition.CombatPlayerId), firstElement.CombatPlayerId.GetType());
        table.Columns.Add(nameof(CombatPlayerPosition.CombatId), firstElement.CombatId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.PositionX,
                item.PositionY,
                item.Time,
                item.CombatPlayerId,
                item.CombatId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(CombatPlayerPosition)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(CombatPlayerPosition)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
