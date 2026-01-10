using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPCombatPlayerPositionRepositoryBatch(CombatParserContext context) : GenericRepository<CombatPlayerPosition>(context), ICreateBatchRepository<CombatPlayerPosition>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<CombatPlayerPosition> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<double>(nameof(CombatPlayerPosition.PositionX));
        table.AddColumn<double>(nameof(CombatPlayerPosition.PositionY));
        table.AddColumn<TimeSpan>(nameof(CombatPlayerPosition.Time));
        table.AddColumn<int>(nameof(CombatPlayerPosition.CombatPlayerId));
        table.AddColumn<int>(nameof(CombatPlayerPosition.CombatId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.PositionX,
                item.PositionY,
                item.Time,
                item.CombatPlayerId,
                item.CombatId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(CombatPlayerPosition)}Type"
        };

        var sql = $"EXEC InsertInto{nameof(CombatPlayerPosition)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
