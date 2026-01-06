using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPDamageTakenGeneralRepositoryBatch(CombatParserContext context) : GenericRepository<DamageTakenGeneral>(context), IGenericRepositoryBatch<DamageTakenGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<DamageTakenGeneral> items)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(DamageTakenGeneral.GameSpellId));
        table.AddColumn<string>(nameof(DamageTakenGeneral.Spell));
        table.AddColumn<int>(nameof(DamageTakenGeneral.Value));
        table.AddColumn<int>(nameof(DamageTakenGeneral.ActualValue));
        table.AddColumn<double>(nameof(DamageTakenGeneral.DamageTakenPerSecond));
        table.AddColumn<int>(nameof(DamageTakenGeneral.CritNumber));
        table.AddColumn<int>(nameof(DamageTakenGeneral.MissNumber));
        table.AddColumn<int>(nameof(DamageTakenGeneral.CastNumber));
        table.AddColumn<int>(nameof(DamageTakenGeneral.MinValue));
        table.AddColumn<int>(nameof(DamageTakenGeneral.MaxValue));
        table.AddColumn<double>(nameof(DamageTakenGeneral.AverageValue));
        table.AddColumn<int>(nameof(DamageTakenGeneral.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.ActualValue,
                item.DamageTakenPerSecond,
                item.CritNumber,
                item.MissNumber,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(DamageTakenGeneral)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(DamageTakenGeneral)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, itemsParam);
    }
}
