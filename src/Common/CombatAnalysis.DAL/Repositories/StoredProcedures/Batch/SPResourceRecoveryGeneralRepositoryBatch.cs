using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPResourceRecoveryGeneralRepositoryBatch(CombatParserContext context) : GenericRepository<ResourceRecoveryGeneral>(context), IGenericRepositoryBatch<ResourceRecoveryGeneral>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<ResourceRecoveryGeneral> items)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>(nameof(ResourceRecoveryGeneral.GameSpellId));
        table.AddColumn<string>(nameof(ResourceRecoveryGeneral.Spell));
        table.AddColumn<int>(nameof(ResourceRecoveryGeneral.Value));
        table.AddColumn<double>(nameof(ResourceRecoveryGeneral.ResourcePerSecond));
        table.AddColumn<int>(nameof(ResourceRecoveryGeneral.CastNumber));
        table.AddColumn<int>(nameof(ResourceRecoveryGeneral.MinValue));
        table.AddColumn<int>(nameof(ResourceRecoveryGeneral.MaxValue));
        table.AddColumn<double>(nameof(ResourceRecoveryGeneral.AverageValue));
        table.AddColumn<int>(nameof(ResourceRecoveryGeneral.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.GameSpellId,
                item.Spell,
                item.Value,
                item.ResourcePerSecond,
                item.CastNumber,
                item.MinValue,
                item.MaxValue,
                item.AverageValue,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(ResourceRecoveryGeneral)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(ResourceRecoveryGeneral)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, itemsParam);
    }
}
