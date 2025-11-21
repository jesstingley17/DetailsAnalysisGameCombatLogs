using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class DamageTakenGeneralTestDataFactory
{
    public static DamageTakenGeneral Create(int id = 1, string spell = "Rush")
    {
        var entity = new DamageTakenGeneral
        {
            Id = id,
            Spell = spell,
            Value = 132324,
            ActualValue = 122324,
            DamageTakenPerSecond = 13402,
            CritNumber = 20,
            MissNumber = 2,
            CastNumber = 40,
            MinValue = 13232,
            MaxValue = 43232,
            AverageValue = 23000,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static DamageTakenGeneralDto CreateDto(int id = 1, string spell = "Rush")
    {
        var entityDto = new DamageTakenGeneralDto
        {
            Id = id,
            Spell = spell,
            Value = 132324,
            ActualValue = 122324,
            DamageTakenPerSecond = 13402,
            CritNumber = 20,
            MissNumber = 2,
            CastNumber = 40,
            MinValue = 13232,
            MaxValue = 43232,
            AverageValue = 23000,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<DamageTakenGeneral> CreateCollection()
    {
        var collection = new List<DamageTakenGeneral>
        {
            new()
            {
                Id = 1,
                Spell = "Melee",
                Value = 12324,
                ActualValue = 122324,
                DamageTakenPerSecond = 33402,
                CritNumber = 20,
                MissNumber = 2,
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
                ActualValue = 122324,
                DamageTakenPerSecond = 23402,
                CritNumber = 20,
                MissNumber = 2,
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
                ActualValue = 122324,
                DamageTakenPerSecond = 4402,
                CritNumber = 20,
                MissNumber = 2,
                CastNumber = 40,
                MinValue = 33232,
                MaxValue = 53232,
                AverageValue = 23000,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<DamageTakenGeneralDto> CreateDtoColelction()
    {
        var collection = new List<DamageTakenGeneralDto>
        {
            new()
            {
                Id = 1,
                Spell = "Melee",
                Value = 12324,
                ActualValue = 122324,
                DamageTakenPerSecond = 33402,
                CritNumber = 20,
                MissNumber = 2,
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
                ActualValue = 122324,
                DamageTakenPerSecond = 23402,
                CritNumber = 20,
                MissNumber = 2,
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
                ActualValue = 122324,
                DamageTakenPerSecond = 4402,
                CritNumber = 20,
                MissNumber = 2,
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
