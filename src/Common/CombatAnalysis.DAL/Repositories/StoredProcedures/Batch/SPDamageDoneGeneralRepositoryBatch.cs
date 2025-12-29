using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPDamageDoneGeneralRepositoryBatch(CombatParserContext context) : SPGenericRepository<DamageDoneGeneral>(context), IGenericRepositoryBatch<DamageDoneGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageDoneGeneral> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(DamageDoneGeneral.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.DamagePerSecond), firstElement.DamagePerSecond.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.CritNumber), firstElement.CritNumber.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.MissNumber), firstElement.MissNumber.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.CastNumber), firstElement.CastNumber.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.MinValue), firstElement.MinValue.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.MaxValue), firstElement.MaxValue.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.AverageValue), firstElement.AverageValue.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.IsPet), firstElement.IsPet.GetType());
        table.Columns.Add(nameof(DamageDoneGeneral.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Spell,
                item.Value,
                item.DamagePerSecond,
                item.CritNumber,
                item.MissNumber,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.IsPet,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageDoneGeneral)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(DamageDoneGeneral)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
