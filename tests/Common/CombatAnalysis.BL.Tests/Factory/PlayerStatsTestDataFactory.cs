using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class PlayerStatsTestDataFactory
{
    public static CombatPlayerStats Create(int id = 1, string talents = "0,0,0,0,0,0")
    {
        var entity = new CombatPlayerStats
        {
            Id = id,
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

    public static CombatPlayerStatsDto CreateDto(int id = 1, string talents = "0,0,0,0,0,0")
    {
        var entityDto = new CombatPlayerStatsDto
        {
            Id = id,
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

    public static List<CombatPlayerStats> CreateCollection()
    {
        var collection = new List<CombatPlayerStats>
        {
            new () {
                Id = 1,
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

    public static List<CombatPlayerStatsDto> CreateDtoCollection()
    {
        var collection = new List<CombatPlayerStatsDto>
        {
            new () {
                Id = 1,
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
