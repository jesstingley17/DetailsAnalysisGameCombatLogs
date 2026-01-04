using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPResourceRecoveryGeneralRepositoryBatch(CombatParserContext context) : SPGenericRepository<ResourceRecoveryGeneral>(context), IGenericRepositoryBatch<ResourceRecoveryGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<ResourceRecoveryGeneral> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(ResourceRecoveryGeneral.GameSpellId), firstElement.GameSpellId.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.ResourcePerSecond), firstElement.ResourcePerSecond.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.CastNumber), firstElement.CastNumber.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.MinValue), firstElement.MinValue.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.MaxValue), firstElement.MaxValue.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.AverageValue), firstElement.AverageValue.GetType());
        table.Columns.Add(nameof(ResourceRecoveryGeneral.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.ResourcePerSecond,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(ResourceRecoveryGeneral)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(ResourceRecoveryGeneral)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
