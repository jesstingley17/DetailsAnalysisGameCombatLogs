using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities.CombatPlayerData;

namespace CombatAnalysis.BL.Tests.Factory;

internal class DamageTakenTestDataFactory
{
    public static DamageTaken Create(int id = 1, string spell = "Rush")
    {
        var entity = new DamageTaken
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Time = TimeSpan.Parse("00:01:23"),
            Creator ="Solinx",
            Target = "Boss",
            DamageTakenType = 0,
            ActualValue = 13500,
            IsPeriodicDamage = false,
            Resisted = 0,
            Absorbed = 23400,
            Blocked = 0,
            RealDamage = 43423,
            Mitigated = 0,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static DamageTakenDto CreateDto(int id = 1, string spell = "Rush")
    {
        var entityDto = new DamageTakenDto
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Time = TimeSpan.Parse("00:01:23"),
            Creator = "Solinx",
            Target = "Boss",
            DamageTakenType = 0,
            ActualValue = 13500,
            IsPeriodicDamage = false,
            Resisted = 0,
            Absorbed = 23400,
            Blocked = 0,
            RealDamage = 43423,
            Mitigated = 0,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<DamageTaken> CreateCollection()
    {
        var collection = new List<DamageTaken>
        {
            new()
            {
                Id = 1,
                Spell = "Rush",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:23"),
                Creator ="Solinx",
                Target = "Boss",
                DamageTakenType = 0,
                ActualValue = 13500,
                IsPeriodicDamage = false,
                Resisted = 0,
                Absorbed = 23400,
                Blocked = 0,
                RealDamage = 43423,
                Mitigated = 0,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Rush",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:26"),
                Creator ="Solinx",
                Target = "Boss",
                DamageTakenType = 0,
                ActualValue = 3500,
                IsPeriodicDamage = false,
                Resisted = 0,
                Absorbed = 0,
                Blocked = 0,
                RealDamage = 143423,
                Mitigated = 0,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Rush",
                Value = 312324,
                Time = TimeSpan.Parse("00:01:31"),
                Creator ="Solinx",
                Target = "Boss",
                DamageTakenType = 0,
                ActualValue = 23500,
                IsPeriodicDamage = false,
                Resisted = 0,
                Absorbed = 3400,
                Blocked = 0,
                RealDamage = 343423,
                Mitigated = 0,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<DamageTakenDto> CreateDtoCollection()
    {
        var collection = new List<DamageTakenDto>
        {
            new()
            {
                Id = 1,
                Spell = "Rush",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:23"),
                Creator ="Solinx",
                Target = "Boss",
                DamageTakenType = 0,
                ActualValue = 13500,
                IsPeriodicDamage = false,
                Resisted = 0,
                Absorbed = 23400,
                Blocked = 0,
                RealDamage = 43423,
                Mitigated = 0,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Rush",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:26"),
                Creator ="Solinx",
                Target = "Boss",
                DamageTakenType = 0,
                ActualValue = 3500,
                IsPeriodicDamage = false,
                Resisted = 0,
                Absorbed = 0,
                Blocked = 0,
                RealDamage = 143423,
                Mitigated = 0,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Rush",
                Value = 312324,
                Time = TimeSpan.Parse("00:01:31"),
                Creator ="Solinx",
                Target = "Boss",
                DamageTakenType = 0,
                ActualValue = 23500,
                IsPeriodicDamage = false,
                Resisted = 0,
                Absorbed = 3400,
                Blocked = 0,
                RealDamage = 343423,
                Mitigated = 0,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
