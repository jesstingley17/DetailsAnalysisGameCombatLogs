using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPResourceRecoveryRepositoryBatch(CombatParserContext context) : GenericRepository<ResourceRecovery>(context), ICreateBatchRepository<ResourceRecovery>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<ResourceRecovery> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(ResourceRecovery.GameSpellId));
        table.AddColumn<string>(nameof(ResourceRecovery.Spell));
        table.AddColumn<int>(nameof(ResourceRecovery.Value));
        table.AddColumn<TimeSpan>(nameof(ResourceRecovery.Time));
        table.AddColumn<string>(nameof(ResourceRecovery.Creator));
        table.AddColumn<string>(nameof(ResourceRecovery.Target));
        table.AddColumn<int>(nameof(ResourceRecovery.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.Time,
                item.Creator,
                item.Target,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(ResourceRecovery)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(ResourceRecovery)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
