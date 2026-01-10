using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPHealDoneRepositoryBatch(CombatParserContext context) : GenericRepository<HealDone>(context), ICreateBatchRepository<HealDone>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<HealDone> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(HealDone.GameSpellId));
        table.AddColumn<string>(nameof(HealDone.Spell));
        table.AddColumn<int>(nameof(HealDone.Value));
        table.AddColumn<TimeSpan>(nameof(HealDone.Time));
        table.AddColumn<string>(nameof(HealDone.Creator));
        table.AddColumn<string>(nameof(HealDone.Target));
        table.AddColumn<int>(nameof(HealDone.Overheal));
        table.AddColumn<bool>(nameof(HealDone.IsCrit));
        table.AddColumn<bool>(nameof(HealDone.IsAbsorbed));
        table.AddColumn<int>(nameof(HealDone.CombatPlayerId));

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

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(HealDone)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(HealDone)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
