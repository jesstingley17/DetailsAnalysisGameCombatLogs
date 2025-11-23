using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class HealDoneGeneralTestDataFactory
{
    public static HealDoneGeneral Create(int id = 1, string spell = "Heal")
    {
        var entity = new HealDoneGeneral
        {
            Id = id,
            Spell = spell,
            Value = 132324,
            HealPerSecond = 13402,
            CritNumber = 20,
            CastNumber = 40,
            MinValue = 13232,
            MaxValue = 43232,
            AverageValue = 23000,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static HealDoneGeneralDto CreateDto(int id = 1, string spell = "Heal")
    {
        var entityDto = new HealDoneGeneralDto
        {
            Id = id,
            Spell = spell,
            Value = 132324,
            HealPerSecond = 13402,
            CritNumber = 20,
            CastNumber = 40,
            MinValue = 13232,
            MaxValue = 43232,
            AverageValue = 23000,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<HealDoneGeneral> CreateCollection()
    {
        var collection = new List<HealDoneGeneral>
        {
            new()
            {
                Id = 1,
                Spell = "Melee",
                Value = 12324,
                HealPerSecond = 33402,
                CritNumber = 20,
                CastNumber = 40,
                MinValue = 23232,
                MaxValue = 41232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Melee",
                Value = 32324,
                HealPerSecond = 23402,
                CritNumber = 20,
                CastNumber = 40,
                MinValue = 3232,
                MaxValue = 43232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Melee",
                Value = 72324,
                HealPerSecond = 4402,
                CritNumber = 20,
                CastNumber = 40,
                MinValue = 33232,
                MaxValue = 53232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<HealDoneGeneralDto> CreateDtoCollection()
    {
        var collection = new List<HealDoneGeneralDto>
        {
            new()
            {
                Id = 1,
                Spell = "Melee",
                Value = 12324,
                HealPerSecond = 33402,
                CritNumber = 20,
                CastNumber = 40,
                MinValue = 23232,
                MaxValue = 41232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Melee",
                Value = 32324,
                HealPerSecond = 23402,
                CritNumber = 20,
                CastNumber = 40,
                MinValue = 3232,
                MaxValue = 43232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Melee",
                Value = 72324,
                HealPerSecond = 4402,
                CritNumber = 20,
                CastNumber = 40,
                MinValue = 33232,
                MaxValue = 53232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
