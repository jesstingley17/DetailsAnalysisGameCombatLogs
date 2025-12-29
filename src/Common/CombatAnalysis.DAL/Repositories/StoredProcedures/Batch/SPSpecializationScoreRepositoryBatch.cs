using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories.StoredProcedures.Batch;

internal class SPSpecializationScoreRepositoryBatch(CombatParserContext context) : SPGenericRepository<SpecializationScore>(context), IGenericRepositoryBatch<SpecializationScore>
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<SpecializationScore> items)
    {
        if (!items.Any())
        {
            return;
        }

        var firstElement = items.First();

        var table = new DataTable();
        table.Columns.Add(nameof(SpecializationScore.SpecId), firstElement.SpecId.GetType());
        table.Columns.Add(nameof(SpecializationScore.BossId), firstElement.BossId.GetType());
        table.Columns.Add(nameof(SpecializationScore.Damage), firstElement.Damage.GetType());
        table.Columns.Add(nameof(SpecializationScore.Heal), firstElement.Heal.GetType());
        table.Columns.Add(nameof(SpecializationScore.Updated), firstElement.Updated.GetType());

        foreach (var item in items)
        {
            table.Rows.Add(
                item.SpecId,
                item.BossId,
                item.Damage,
                item.Heal,
                item.Updated);
        }

        var param = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(SpecializationScore)}Type"
        };

        var storedProcedureName = $"InsertInto{nameof(SpecializationScore)}Batch";
        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC {storedProcedureName} {param}");
    }
}
