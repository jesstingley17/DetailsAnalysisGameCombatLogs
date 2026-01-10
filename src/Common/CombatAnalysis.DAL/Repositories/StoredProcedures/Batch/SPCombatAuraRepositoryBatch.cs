using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPCombatAuraRepositoryBatch(CombatParserContext context) : GenericRepository<CombatAura>(context), ICreateBatchRepository<CombatAura>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<CombatAura> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<string>(nameof(CombatAura.Name));
        table.AddColumn<string>(nameof(CombatAura.Creator));
        table.AddColumn<string>(nameof(CombatAura.Target));
        table.AddColumn<int>(nameof(CombatAura.AuraCreatorType));
        table.AddColumn<int>(nameof(CombatAura.AuraType));
        table.AddColumn<TimeSpan>(nameof(CombatAura.StartTime));
        table.AddColumn<TimeSpan>(nameof(CombatAura.FinishTime));
        table.AddColumn<int>(nameof(CombatAura.Stacks));
        table.AddColumn<int>(nameof(CombatAura.CombatId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Name,
                item.Creator,
                item.Target,
                item.AuraCreatorType,
                item.AuraType,
                item.StartTime,
                item.FinishTime,
                item.Stacks,
                item.CombatId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(CombatAura)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(CombatAura)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
