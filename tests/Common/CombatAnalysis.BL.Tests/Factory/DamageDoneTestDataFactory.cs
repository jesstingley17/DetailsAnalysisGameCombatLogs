using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class DamageDoneTestDataFactory
{
    public static DamageDone Create(int id = 1, string spell = "Test spell")
    {
        var entity = new DamageDone
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Time = TimeSpan.Parse("00:01:23"),
            Creator ="Solinx",
            Target = "Boss",
            DamageType = 0,
            IsPeriodicDamage = false,
            IsPet = false,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static DamageDoneDto CreateDto(int id = 1, string spell = "Test spell")
    {
        var entityDto = new DamageDoneDto
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Time = TimeSpan.Parse("00:01:23"),
            Creator = "Solinx",
            Target = "Boss",
            DamageType = 0,
            IsPeriodicDamage = false,
            IsPet = false,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<DamageDone> CreateCollection()
    {
        var collection = new List<DamageDone>
        {
            new()
            {
                Id = 1,
                Spell = "Test spell",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:23"),
                Creator = "Solinx",
                Target = "Boss",
                DamageType = 0,
                IsPeriodicDamage = false,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Test spell 2",
                Value = 23324,
                Time = TimeSpan.Parse("00:01:43"),
                Creator = "Solinx",
                Target = "Boss",
                DamageType = 0,
                IsPeriodicDamage = false,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Test spell 3",
                Value = 132324,
                Time = TimeSpan.Parse("00:01:53"),
                Creator = "Solinx",
                Target = "Boss",
                DamageType = 0,
                IsPeriodicDamage = true,
                IsPet = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<DamageDoneDto> CreateDtoCollection()
    {
        var collection = new List<DamageDoneDto>
        {
            new()
            {
                Id = 1,
                Spell = "Test spell",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:23"),
                Creator = "Solinx",
                Target = "Boss",
                DamageType = 0,
                IsPeriodicDamage = false,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Test spell 2",
                Value = 23324,
                Time = TimeSpan.Parse("00:01:43"),
                Creator = "Solinx",
                Target = "Boss",
                DamageType = 0,
                IsPeriodicDamage = false,
                IsPet = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Test spell 3",
                Value = 132324,
                Time = TimeSpan.Parse("00:01:53"),
                Creator = "Solinx",
                Target = "Boss",
                DamageType = 0,
                IsPeriodicDamage = true,
                IsPet = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
