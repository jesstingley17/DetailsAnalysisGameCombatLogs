using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPDamageDoneRepositoryBatch(CombatParserContext context) : GenericRepository<DamageDone>(context), ICreateBatchRepository<DamageDone>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageDone> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(DamageDone.GameSpellId));
        table.AddColumn<string>(nameof(DamageDone.Spell));
        table.AddColumn<int>(nameof(DamageDone.Value));
        table.AddColumn<TimeSpan>(nameof(DamageDone.Time));
        table.AddColumn<string>(nameof(DamageDone.Creator));
        table.AddColumn<string>(nameof(DamageDone.Target));
        table.AddColumn<bool>(nameof(DamageDone.IsTargetBoss));
        table.AddColumn<int>(nameof(DamageDone.DamageType));
        table.AddColumn<bool>(nameof(DamageDone.IsPeriodicDamage));
        table.AddColumn<bool>(nameof(DamageDone.IsSingleTarget));
        table.AddColumn<bool>(nameof(DamageDone.IsPet));
        table.AddColumn<int>(nameof(DamageDone.CombatPlayerId));

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

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageDone)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(DamageDone)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
