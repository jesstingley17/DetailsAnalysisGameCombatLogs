using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories;

internal class SpecializationScoreRepository(CombatParserContext context) : ISpecializationScoreRepository
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<SpecializationScore> items)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<double>(nameof(SpecializationScore.DamageScore));
        table.AddColumn<int>(nameof(SpecializationScore.DamageDone));
        table.AddColumn<double>(nameof(SpecializationScore.HealScore));
        table.AddColumn<int>(nameof(SpecializationScore.HealDone));
        table.AddColumn<DateTimeOffset>(nameof(SpecializationScore.Updated));
        table.AddColumn<int>(nameof(SpecializationScore.SpecializationId));
        table.AddColumn<int>(nameof(SpecializationScore.CombatPlayerId));

        foreach (var item in items)
        {
            table.Rows.Add(
                item.DamageScore,
                item.DamageDone,
                item.HealScore,
                item.HealDone,
                item.Updated,
                item.SpecializationId,
                item.CombatPlayerId);
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(SpecializationScore)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(SpecializationScore)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, itemsParam);
    }

    public async Task<int> UpdateAsync(SpecializationScore item)
    {
        var existing = await _context.Set<SpecializationScore>().FindAsync(item.Id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<SpecializationScore>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Set<SpecializationScore>().Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<SpecializationScore?> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var data = await _context.Set<SpecializationScore>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CombatPlayerId == combatPlayerId);

        return data;
    }
}
