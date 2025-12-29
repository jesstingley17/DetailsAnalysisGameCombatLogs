using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPPlayerParseInfoRepositoryBatch(CombatParserContext context) : SPGenericRepository<PlayerParseInfo>(context), IGenericRepositoryBatch<PlayerParseInfo>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<PlayerParseInfo> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(PlayerParseInfo.SpecId), firstElement.SpecId.GetType());
        table.Columns.Add(nameof(PlayerParseInfo.ClassId), firstElement.ClassId.GetType());
        table.Columns.Add(nameof(PlayerParseInfo.BossId), firstElement.BossId.GetType());
        table.Columns.Add(nameof(PlayerParseInfo.DamageEfficiency), firstElement.DamageEfficiency.GetType());
        table.Columns.Add(nameof(PlayerParseInfo.HealEfficiency), firstElement.HealEfficiency.GetType());
        table.Columns.Add(nameof(PlayerParseInfo.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.SpecId,
                item.ClassId,
                item.BossId,
                item.DamageEfficiency,
                item.HealEfficiency,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(PlayerParseInfo)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(PlayerParseInfo)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
