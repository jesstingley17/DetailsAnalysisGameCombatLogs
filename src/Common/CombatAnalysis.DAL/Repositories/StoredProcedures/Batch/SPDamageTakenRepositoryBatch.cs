using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures;

internal class SPDamageTakenRepositoryBatch(CombatParserContext context) : GenericRepository<DamageTaken>(context), IGenericRepositoryBatch<DamageTaken>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageTaken> items)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(DamageTaken.GameSpellId));
        table.AddColumn<string>(nameof(DamageTaken.Spell));
        table.AddColumn<int>(nameof(DamageTaken.Value));
        table.AddColumn<TimeSpan>(nameof(DamageTaken.Time));
        table.AddColumn<string>(nameof(DamageTaken.Creator));
        table.AddColumn<string>(nameof(DamageTaken.Target));
        table.AddColumn<int>(nameof(DamageTaken.DamageTakenType));
        table.AddColumn<int>(nameof(DamageTaken.ActualValue));
        table.AddColumn<bool>(nameof(DamageTaken.IsPeriodicDamage));
        table.AddColumn<int>(nameof(DamageTaken.Resisted));
        table.AddColumn<int>(nameof(DamageTaken.Absorbed));
        table.AddColumn<int>(nameof(DamageTaken.Blocked));
        table.AddColumn<int>(nameof(DamageTaken.RealDamage));
        table.AddColumn<int>(nameof(DamageTaken.Mitigated));
        table.AddColumn<int>(nameof(DamageTaken.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.Time,
                item.Creator,
                item.Target,
                item.DamageTakenType,
                item.ActualValue,
                item.IsPeriodicDamage,
                item.Resisted,
                item.Absorbed,
                item.Blocked,
                item.RealDamage,
                item.Mitigated,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageTaken)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(DamageTaken)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, itemsParam);
    }
}
