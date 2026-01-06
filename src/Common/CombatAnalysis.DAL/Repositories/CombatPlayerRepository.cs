using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories;

internal class CombatPlayerRepository(CombatParserContext context) : ICombatPlayerRepository
{
    private readonly CombatParserContext _context = context;

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

                Score = new SpecializationScore
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

                Stats = new CombatPlayerStats
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
                }
            })
            .ToListAsync();

        return result.Count != 0 ? result : [];
    }
}
