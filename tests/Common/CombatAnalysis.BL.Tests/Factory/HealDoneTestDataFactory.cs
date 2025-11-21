using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class HealDoneTestDataFactory
{
    public static HealDone Create(int id = 1, string spell = "Holy light")
    {
        var entity = new HealDone
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Overheal = 234,
            Time = TimeSpan.Parse("00:01:23"),
            Creator ="Solinx",
            Target = "Boss",
            IsCrit = false,
            IsAbsorbed = false,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static HealDoneDto CreateDto(int id = 1, string spell = "Holy light")
    {
        var entityDto = new HealDoneDto
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Overheal = 234,
            Time = TimeSpan.Parse("00:01:23"),
            Creator = "Solinx",
            Target = "Boss",
            IsCrit = false,
            IsAbsorbed = false,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<HealDone> CreateCollection()
    {
        var collection = new List<HealDone>
        {
            new()
            {
                Id = 1,
                Spell = "Holy light",
                Value = 232324,
                Overheal = 234,
                Time = TimeSpan.Parse("00:01:23"),
                Creator = "Solinx",
                Target = "Boss",
                IsCrit = false,
                IsAbsorbed = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Holy light",
                Value = 22324,
                Overheal = 0,
                Time = TimeSpan.Parse("00:01:44"),
                Creator = "Solinx",
                Target = "Boss",
                IsCrit = false,
                IsAbsorbed = true,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Holy light",
                Value = 142324,
                Overheal = 1234,
                Time = TimeSpan.Parse("00:02:50"),
                Creator = "Solinx",
                Target = "Boss",
                IsCrit = true,
                IsAbsorbed = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<HealDoneDto> CreateDtoColelction()
    {
        var collection = new List<HealDoneDto>
        {
            new()
            {
                Id = 1,
                Spell = "Holy light",
                Value = 232324,
                Overheal = 234,
                Time = TimeSpan.Parse("00:01:23"),
                Creator = "Solinx",
                Target = "Boss",
                IsCrit = false,
                IsAbsorbed = false,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Holy light",
                Value = 22324,
                Overheal = 0,
                Time = TimeSpan.Parse("00:01:44"),
                Creator = "Solinx",
                Target = "Boss",
                IsCrit = false,
                IsAbsorbed = true,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Holy light",
                Value = 142324,
                Overheal = 1234,
                Time = TimeSpan.Parse("00:02:50"),
                Creator = "Solinx",
                Target = "Boss",
                IsCrit = true,
                IsAbsorbed = false,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
