using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class ResourceRecoveryTestDataFactory
{
    public static ResourceRecovery Create(int id = 1, string spell = "Test spell")
    {
        var entity = new ResourceRecovery
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Time = TimeSpan.Parse("00:01:23"),
            Creator ="Solinx",
            Target = "Boss",
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static ResourceRecoveryDto CreateDto(int id = 1, string spell = "Test spell")
    {
        var entityDto = new ResourceRecoveryDto
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            Time = TimeSpan.Parse("00:01:23"),
            Creator = "Solinx",
            Target = "Boss",
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<ResourceRecovery> CreateCollection()
    {
        var collection = new List<ResourceRecovery>
        {
            new()
            {
                Id = 1,
                Spell = "Test",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:02"),
                Creator ="Solinx",
                Target = "Player",
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Test",
                Value = 23324,
                Time = TimeSpan.Parse("00:01:17"),
                Creator ="Solinx",
                Target = "Player",
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Test",
                Value = 132324,
                Time = TimeSpan.Parse("00:01:28"),
                Creator ="Solinx",
                Target = "Player",
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<ResourceRecoveryDto> CreateDtoCollection()
    {
        var collection = new List<ResourceRecoveryDto>
        {
            new()
            {
                Id = 1,
                Spell = "Test",
                Value = 232324,
                Time = TimeSpan.Parse("00:01:02"),
                Creator ="Solinx",
                Target = "Player",
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Test",
                Value = 23324,
                Time = TimeSpan.Parse("00:01:17"),
                Creator ="Solinx",
                Target = "Player",
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Test",
                Value = 132324,
                Time = TimeSpan.Parse("00:01:28"),
                Creator ="Solinx",
                Target = "Player",
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
