using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Extensions;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CombatAnalysis.DAL.Repositories;

internal class CombatPlayerRepository(CombatParserContext context) : ICombatPlayerRepository
{
    private readonly CombatParserContext _context = context;

    public async Task CreateBatchAsync(IEnumerable<CombatPlayer> items)
    {
        if (!items.Any())
        {
            return;
        }

        var table = new DataTable();
        table.AddColumn<int>("RowId");
        table.AddColumn<double>(nameof(CombatPlayer.AverageItemLevel));
        table.AddColumn<int>(nameof(CombatPlayer.ResourcesRecovery));
        table.AddColumn<int>(nameof(CombatPlayer.DamageDone));
        table.AddColumn<int>(nameof(CombatPlayer.HealDone));
        table.AddColumn<int>(nameof(CombatPlayer.DamageTaken));
        table.AddColumn<string>(nameof(CombatPlayer.PlayerId));
        table.AddColumn<int>(nameof(CombatPlayer.CombatId));

        table.AddColumn<int>(nameof(CombatPlayerStats.Strength));
        table.AddColumn<int>(nameof(CombatPlayerStats.Agility));
        table.AddColumn<int>(nameof(CombatPlayerStats.Intelligence));
        table.AddColumn<int>(nameof(CombatPlayerStats.Stamina));
        table.AddColumn<int>(nameof(CombatPlayerStats.Spirit));
        table.AddColumn<int>(nameof(CombatPlayerStats.Dodge));
        table.AddColumn<int>(nameof(CombatPlayerStats.Parry));
        table.AddColumn<int>(nameof(CombatPlayerStats.Crit));
        table.AddColumn<int>(nameof(CombatPlayerStats.Haste));
        table.AddColumn<int>(nameof(CombatPlayerStats.Hit));
        table.AddColumn<int>(nameof(CombatPlayerStats.Expertise));
        table.AddColumn<int>(nameof(CombatPlayerStats.Armor));
        table.AddColumn<string>(nameof(CombatPlayerStats.Talents));

        table.AddColumn<double>(nameof(SpecializationScore.DamageScore), false);
        table.AddColumn<double>(nameof(SpecializationScore.HealScore), false);
        table.AddColumn<DateTimeOffset>(nameof(SpecializationScore.Updated), false);
        table.AddColumn<int>(nameof(SpecializationScore.SpecializationId), false);

        var rowId = 1;
        foreach (var item in items)
        {
            table.Rows.Add(
                rowId++,
                item.AverageItemLevel,
                item.ResourcesRecovery,
                item.DamageDone,
                item.HealDone,
                item.DamageTaken,
                item.PlayerId,
                item.CombatId,
                item.Stats.Strength,
                item.Stats.Agility,
                item.Stats.Intelligence,
                item.Stats.Stamina,
                item.Stats.Spirit,
                item.Stats.Dodge,
                item.Stats.Parry,
                item.Stats.Crit,
                item.Stats.Haste,
                item.Stats.Hit,
                item.Stats.Expertise,
                item.Stats.Armor,
                item.Stats.Talents,
                item.Score?.DamageScore,
                item.Score?.HealScore,
                item.Score?.Updated,
                item.Score?.SpecializationId
                );
        }

        var itemsParam = new SqlParameter("@Items", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = $"dbo.{nameof(CombatPlayer)}Type"
        };

        var sql = $"EXEC dbo.InsertInto{nameof(CombatPlayer)}Batch @Items";
        await _context.Database.ExecuteSqlRawAsync(sql, itemsParam);
    }

    public async Task<CombatPlayer> CreateAsync(CombatPlayer item)
    {
        _context.Set<Player>().Attach(item.Player);

        var entityEntry = await _context.Set<CombatPlayer>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> UpdateAsync(int id, CombatPlayer item)
    {
        var existing = await _context.Set<CombatPlayer>().FindAsync(id) ?? throw new KeyNotFoundException();
        _context.Entry(existing).CurrentValues.SetValues(item);

        return await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId)
    {
        var result = await _context.Set<CombatPlayer>()
            .AsNoTracking()
            .Where(cp => cp.CombatId == combatId)
            .Select(cp => new CombatPlayer
            {
                Id = cp.Id,
                CombatId = cp.CombatId,
                AverageItemLevel = cp.AverageItemLevel,
                DamageDone = cp.DamageDone,
                HealDone = cp.HealDone,
                DamageTaken = cp.DamageTaken,
                ResourcesRecovery = cp.ResourcesRecovery,

                Player = new Player
                {
                    Id = cp.Player.Id,
                    GameId = cp.Player.GameId,
                    Username = cp.Player.Username,
                    Faction = cp.Player.Faction,
                },

                Stats = cp.Stats == null ? null : new CombatPlayerStats
                {
                    Id = cp.Stats.Id,
                    Strength = cp.Stats.Strength,
                    Agility = cp.Stats.Agility,
                    Intelligence = cp.Stats.Intelligence,
                    Stamina = cp.Stats.Stamina,
                    Spirit = cp.Stats.Spirit,
                    Dodge = cp.Stats.Dodge,
                    Parry = cp.Stats.Parry,
                    Crit = cp.Stats.Crit,
                    Haste = cp.Stats.Haste,
                    Hit = cp.Stats.Hit,
                    Expertise = cp.Stats.Expertise,
                    Armor = cp.Stats.Armor,
                    Talents = cp.Stats.Talents,
                    CombatPlayerId = cp.Stats.CombatPlayerId,
                },

                Score = cp.Score == null ? null : new SpecializationScore
                {
                    Id = cp.Score.Id,
                    DamageScore = cp.Score.DamageScore,
                    DamageDone = cp.Score.DamageDone,
                    HealScore = cp.Score.HealDone,
                    HealDone = cp.Score.HealDone,
                    Updated = cp.Score.Updated,
                    SpecializationId = cp.Score.SpecializationId,
                    CombatPlayerId = cp.Score.CombatPlayerId,
                },
            })
            .ToListAsync();

        return result.Count != 0 ? result : [];
    }
}
