using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPHealDoneGeneralRepositoryBatch(CombatParserContext context) : GenericRepository<HealDoneGeneral>(context), ICreateBatchRepository<HealDoneGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<HealDoneGeneral> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(HealDoneGeneral.GameSpellId));
        table.AddColumn<string>(nameof(HealDoneGeneral.Spell));
        table.AddColumn<int>(nameof(HealDoneGeneral.Value));
        table.AddColumn<double>(nameof(HealDoneGeneral.HealPerSecond));
        table.AddColumn<int>(nameof(HealDoneGeneral.CritNumber));
        table.AddColumn<int>(nameof(HealDoneGeneral.CastNumber));
        table.AddColumn<int>(nameof(HealDoneGeneral.MinValue));
        table.AddColumn<int>(nameof(HealDoneGeneral.MaxValue));
        table.AddColumn<double>(nameof(HealDoneGeneral.AverageValue));
        table.AddColumn<int>(nameof(HealDoneGeneral.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
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

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(HealDoneGeneral)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(HealDoneGeneral)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
