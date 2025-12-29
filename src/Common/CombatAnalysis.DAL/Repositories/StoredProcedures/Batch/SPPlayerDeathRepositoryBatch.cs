using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPPlayerDeathRepositoryBatch(CombatParserContext context) : SPGenericRepository<PlayerDeath>(context), IGenericRepositoryBatch<PlayerDeath>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<PlayerDeath> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(PlayerDeath.Username), firstElement.Username.GetType());
        table.Columns.Add(nameof(PlayerDeath.LastHitSpellOrItem), firstElement.LastHitSpellOrItem.GetType());
        table.Columns.Add(nameof(PlayerDeath.LastHitValue), firstElement.LastHitValue.GetType());
        table.Columns.Add(nameof(PlayerDeath.Time), firstElement.Time.GetType());
        table.Columns.Add(nameof(PlayerDeath.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Username,
                item.LastHitSpellOrItem,
                item.LastHitValue,
                item.Time,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(PlayerDeath)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(PlayerDeath)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
