using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPPlayerDeathRepositoryBatch(CombatParserContext context) : GenericRepository<CombatPlayerDeath>(context), IGenericRepositoryBatch<CombatPlayerDeath>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<CombatPlayerDeath> items)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<string>(nameof(CombatPlayerDeath.Username));
        table.AddColumn<string>(nameof(CombatPlayerDeath.LastHitSpell));
        table.AddColumn<int>(nameof(CombatPlayerDeath.LastHitValue));
        table.AddColumn<TimeSpan>(nameof(CombatPlayerDeath.Time));
        table.AddColumn<int>(nameof(CombatPlayerDeath.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.Username,
                item.LastHitSpell,
                item.LastHitValue,
                item.Time,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(CombatPlayerDeath)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(CombatPlayerDeath)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, itemsParam);
    }
}
