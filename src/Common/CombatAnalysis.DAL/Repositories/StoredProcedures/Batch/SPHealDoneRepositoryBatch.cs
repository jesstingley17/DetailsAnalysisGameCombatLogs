using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPHealDoneRepositoryBatch(CombatParserContext context) : SPGenericRepository<HealDone>(context), IGenericRepositoryBatch<HealDone>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<HealDone> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(HealDone.GameSpellId), firstElement.GameSpellId.GetType());
        table.Columns.Add(nameof(HealDone.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(HealDone.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(HealDone.Time), firstElement.Time.GetType());
        table.Columns.Add(nameof(HealDone.Creator), firstElement.Creator.GetType());
        table.Columns.Add(nameof(HealDone.Target), firstElement.Target.GetType());
        table.Columns.Add(nameof(HealDone.Overheal), firstElement.Overheal.GetType());
        table.Columns.Add(nameof(HealDone.IsCrit), firstElement.IsCrit.GetType());
        table.Columns.Add(nameof(HealDone.IsAbsorbed), firstElement.IsAbsorbed.GetType());
        table.Columns.Add(nameof(HealDone.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.Time,
                item.Creator,
                item.Target,
                item.Overheal,
                item.IsCrit,
                item.IsAbsorbed,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(HealDone)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(HealDone)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
