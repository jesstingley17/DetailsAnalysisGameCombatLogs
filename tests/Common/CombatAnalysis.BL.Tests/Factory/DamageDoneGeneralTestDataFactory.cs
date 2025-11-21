using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class DamageDoneGeneralTestDataFactory
{
    public static DamageDoneGeneral Create(int id = 1, string spell = "Range attack")
    {
        var entity = new DamageDoneGeneral
        {
            Id = id,
            Spell = spell,
            Value = 132324,
            DamagePerSecond = 13402,
            CritNumber = 20,
            MissNumber = 2,
            CastNumber = 40,
            MinValue = 13232,
            MaxValue = 43232,
            AverageValue = 23000,
            IsPet = false,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static DamageDoneGeneralDto CreateDto(int id = 1, string spell = "Range attack")
    {
        var entityDto = new DamageDoneGeneralDto
        {
            Id = id,
            Spell = spell,
            Value = 132324,
            DamagePerSecond = 13402,
            CritNumber = 20,
            MissNumber = 2,
            CastNumber = 40,
            MinValue = 13232,
            MaxValue = 43232,
            AverageValue = 23000,
            IsPet = false,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<DamageDoneGeneral> CreateCollection()
    {
        var collection = new List<DamageDoneGeneral>
        {
            new()
            {
                Id = 1,
                Spell = "Range attack",
                Value = 132324,
                DamagePerSecond = 13402,
                CritNumber = 20,
                MissNumber = 2,
                CastNumber = 40,
                MinValue = 13232,
                MaxValue = 43232,
                AverageValue = 23000,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Range attack",
                Value = 112324,
                DamagePerSecond = 11402,
                CritNumber = 10,
                MissNumber = 0,
                CastNumber = 30,
                MinValue = 13232,
                MaxValue = 43232,
                AverageValue = 23000,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Range attack",
                Value = 13324,
                DamagePerSecond = 12402,
                CritNumber = 0,
                MissNumber = 4,
                CastNumber = 35,
                MinValue = 13232,
                MaxValue = 43232,
                AverageValue = 23000,
                IsPet = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<DamageDoneGeneralDto> CreateDtoColelction()
    {
        var collection = new List<DamageDoneGeneralDto>
        {
            new()
            {
                Id = 1,
                Spell = "Range attack",
                Value = 132324,
                DamagePerSecond = 13402,
                CritNumber = 20,
                MissNumber = 2,
                CastNumber = 40,
                MinValue = 13232,
                MaxValue = 43232,
                AverageValue = 23000,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Range attack",
                Value = 112324,
                DamagePerSecond = 11402,
                CritNumber = 10,
                MissNumber = 0,
                CastNumber = 30,
                MinValue = 13232,
                MaxValue = 43232,
                AverageValue = 23000,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Range attack",
                Value = 13324,
                DamagePerSecond = 12402,
                CritNumber = 0,
                MissNumber = 4,
                CastNumber = 35,
                MinValue = 13232,
                MaxValue = 43232,
                AverageValue = 23000,
                IsPet = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
