using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPHealDoneGeneralRepositoryBatch(CombatParserContext context) : SPGenericRepository<HealDoneGeneral>(context), IGenericRepositoryBatch<HealDoneGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<HealDoneGeneral> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(HealDoneGeneral.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.HealPerSecond), firstElement.HealPerSecond.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.CritNumber), firstElement.CritNumber.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.CastNumber), firstElement.CastNumber.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.MinValue), firstElement.MinValue.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.MaxValue), firstElement.MaxValue.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.AverageValue), firstElement.AverageValue.GetType());
        table.Columns.Add(nameof(HealDoneGeneral.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Spell,
                item.Value,
                item.HealPerSecond,
                item.CritNumber,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(HealDoneGeneral)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(HealDoneGeneral)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
