using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPDamageTakenGeneralRepositoryBatch(CombatParserContext context) : SPGenericRepository<DamageTakenGeneral>(context), IGenericRepositoryBatch<DamageTakenGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageTakenGeneral> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(DamageTakenGeneral.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.ActualValue), firstElement.ActualValue.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.DamageTakenPerSecond), firstElement.DamageTakenPerSecond.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.CritNumber), firstElement.CritNumber.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.MissNumber), firstElement.MissNumber.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.CastNumber), firstElement.CastNumber.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.MinValue), firstElement.MinValue.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.MaxValue), firstElement.MaxValue.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.AverageValue), firstElement.AverageValue.GetType());
        table.Columns.Add(nameof(DamageTakenGeneral.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Spell,
                item.Value,
                item.ActualValue,
                item.DamageTakenPerSecond,
                item.CritNumber,
                item.MissNumber,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageTakenGeneral)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(DamageTakenGeneral)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
