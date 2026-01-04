using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPDamageDoneRepositoryBatch(CombatParserContext context) : SPGenericRepository<DamageDone>(context), IGenericRepositoryBatch<DamageDone>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageDone> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(DamageDone.GameSpellId), firstElement.GameSpellId.GetType());
        table.Columns.Add(nameof(DamageDone.Spell), firstElement.Spell.GetType());
        table.Columns.Add(nameof(DamageDone.Value), firstElement.Value.GetType());
        table.Columns.Add(nameof(DamageDone.Time), firstElement.Time.GetType());
        table.Columns.Add(nameof(DamageDone.Creator), firstElement.Creator.GetType());
        table.Columns.Add(nameof(DamageDone.Target), firstElement.Target.GetType());
        table.Columns.Add(nameof(DamageDone.IsTargetBoss), firstElement.IsTargetBoss.GetType());
        table.Columns.Add(nameof(DamageDone.DamageType), firstElement.DamageType.GetType());
        table.Columns.Add(nameof(DamageDone.IsPeriodicDamage), firstElement.IsPeriodicDamage.GetType());
        table.Columns.Add(nameof(DamageDone.IsSingleTarget), firstElement.IsSingleTarget.GetType());
        table.Columns.Add(nameof(DamageDone.IsPet), firstElement.IsPet.GetType());
        table.Columns.Add(nameof(DamageDone.CombatPlayerId), firstElement.CombatPlayerId.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.Time,
                item.Creator,
                item.Target,
                item.IsTargetBoss,
                item.DamageType,
                item.IsPeriodicDamage,
                item.IsSingleTarget,
                item.IsPet,
                item.CombatPlayerId);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageDone)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(DamageDone)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
