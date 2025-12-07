using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class PlayerStatsTestDataFactory
{
    public static PlayerStats Create(int id = 1, string talents = "0,0,0,0,0,0")
    {
        var entity = new PlayerStats
        {
            Id = id,
            Faction = 0,
            Strength = 111,
            Agility = 111,
            Intelligence = 111,
            Stamina = 111,
            Spirit = 111,
            Dodge = 111,
            Parry = 111,
            Crit = 111,
            Haste = 111,
            Hit = 111,
            Expertise = 111,
            Armor = 111,
            Talents = talents,
            CombatPlayerId = 1
        };

        return entity;
    }

    public static PlayerStatsDto CreateDto(int id = 1, string talents = "0,0,0,0,0,0")
    {
        var entityDto = new PlayerStatsDto
        {
            Id = id,
            Faction = 0,
            Strength = 111,
            Agility = 111,
            Intelligence = 111,
            Stamina = 111,
            Spirit = 111,
            Dodge = 111,
            Parry = 111,
            Crit = 111,
            Haste = 111,
            Hit = 111,
            Expertise = 111,
            Armor = 111,
            Talents = talents,
            CombatPlayerId = 1
        };

        return entityDto;
    }

    public static List<PlayerStats> CreateCollection()
    {
        var collection = new List<PlayerStats>
        {
            new () {
                Id = 1,
                Faction = 0,
                Strength = 111,
                Agility = 111,
                Intelligence = 111,
                Stamina = 111,
                Spirit = 111,
                Dodge = 111,
                Parry = 111,
                Crit = 111,
                Haste = 111,
                Hit = 111,
                Expertise = 111,
                Armor = 111,
                Talents = "0,0,0,0,0,0",
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                Faction = 0,
                Strength = 111,
                Agility = 111,
                Intelligence = 111,
                Stamina = 111,
                Spirit = 111,
                Dodge = 111,
                Parry = 111,
                Crit = 111,
                Haste = 111,
                Hit = 111,
                Expertise = 111,
                Armor = 111,
                Talents = "0,0,0,0,0,0",
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                Faction = 0,
                Strength = 111,
                Agility = 111,
                Intelligence = 111,
                Stamina = 111,
                Spirit = 111,
                Dodge = 111,
                Parry = 111,
                Crit = 111,
                Haste = 111,
                Hit = 111,
                Expertise = 111,
                Armor = 111,
                Talents = "0,0,0,0,0,0",
                CombatPlayerId = 1
            }
        };

        return collection;
    }

    public static List<PlayerStatsDto> CreateDtoCollection()
    {
        var collection = new List<PlayerStatsDto>
        {
            new () {
                Id = 1,
                Faction = 0,
                Strength = 111,
                Agility = 111,
                Intelligence = 111,
                Stamina = 111,
                Spirit = 111,
                Dodge = 111,
                Parry = 111,
                Crit = 111,
                Haste = 111,
                Hit = 111,
                Expertise = 111,
                Armor = 111,
                Talents = "0,0,0,0,0,0",
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                Faction = 0,
                Strength = 111,
                Agility = 111,
                Intelligence = 111,
                Stamina = 111,
                Spirit = 111,
                Dodge = 111,
                Parry = 111,
                Crit = 111,
                Haste = 111,
                Hit = 111,
                Expertise = 111,
                Armor = 111,
                Talents = "0,0,0,0,0,0",
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                Faction = 0,
                Strength = 111,
                Agility = 111,
                Intelligence = 111,
                Stamina = 111,
                Spirit = 111,
                Dodge = 111,
                Parry = 111,
                Crit = 111,
                Haste = 111,
                Hit = 111,
                Expertise = 111,
                Armor = 111,
                Talents = "0,0,0,0,0,0",
                CombatPlayerId = 1
            }
        };

        return collection;
    }
}
