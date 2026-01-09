using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPDamageDoneGeneralRepositoryBatch(CombatParserContext context) : GenericRepository<DamageDoneGeneral>(context), ICreateBatchRepository<DamageDoneGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageDoneGeneral> items, CancellationToken cancellationToken)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(DamageDoneGeneral.GameSpellId));
        table.AddColumn<string>(nameof(DamageDoneGeneral.Spell));
        table.AddColumn<int>(nameof(DamageDoneGeneral.Value));
        table.AddColumn<double>(nameof(DamageDoneGeneral.DamagePerSecond));
        table.AddColumn<int>(nameof(DamageDoneGeneral.CritNumber));
        table.AddColumn<int>(nameof(DamageDoneGeneral.MissNumber));
        table.AddColumn<int>(nameof(DamageDoneGeneral.CastNumber));
        table.AddColumn<int>(nameof(DamageDoneGeneral.MinValue));
        table.AddColumn<int>(nameof(DamageDoneGeneral.MaxValue));
        table.AddColumn<double>(nameof(DamageDoneGeneral.AverageValue));
        table.AddColumn<bool>(nameof(DamageDoneGeneral.IsPet));
        table.AddColumn<int>(nameof(DamageDoneGeneral.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.DamagePerSecond,
                item.CritNumber,
                item.MissNumber,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.IsPet,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageDoneGeneral)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(DamageDoneGeneral)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, [itemsParam], cancellationToken);
    }
}
