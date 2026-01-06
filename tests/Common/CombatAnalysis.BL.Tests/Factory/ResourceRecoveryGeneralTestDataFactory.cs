using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities.CombatPlayerData;

namespace CombatAnalysis.BL.Tests.Factory;

internal class ResourceRecoveryGeneralTestDataFactory
{
    public static ResourceRecoveryGeneral Create(int id = 1, string spell = "Test spell")
    {
        var entity = new ResourceRecoveryGeneral
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            ResourcePerSecond = 3421,
            CastNumber = 34,
            MinValue = 100,
            MaxValue = 1230,
            AverageValue = 145,
            CombatPlayerId = 1,
        };

        return entity;
    }

    public static ResourceRecoveryGeneralDto CreateDto(int id = 1, string spell = "Test spell")
    {
        var entityDto = new ResourceRecoveryGeneralDto
        {
            Id = id,
            Spell = spell,
            Value = 232324,
            ResourcePerSecond = 3421,
            CastNumber = 34,
            MinValue = 100,
            MaxValue = 1230,
            AverageValue = 145,
            CombatPlayerId = 1,
        };

        return entityDto;
    }

    public static List<ResourceRecoveryGeneral> CreateCollection()
    {
        var collection = new List<ResourceRecoveryGeneral>
        {
            new()
            {
                Id = 1,
                Spell = "Resource",
                Value = 232324,
                ResourcePerSecond = 3421,
                CastNumber = 34,
                MinValue = 100,
                MaxValue = 1230,
                AverageValue = 145,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Resource",
                Value = 232324,
                ResourcePerSecond = 2421,
                CastNumber = 45,
                MinValue = 120,
                MaxValue = 6230,
                AverageValue = 937,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Resource",
                Value = 232324,
                ResourcePerSecond = 10421,
                CastNumber = 24,
                MinValue = 1300,
                MaxValue = 11230,
                AverageValue = 1450,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }

    public static List<ResourceRecoveryGeneralDto> CreateDtoCollection()
    {
        var collection = new List<ResourceRecoveryGeneralDto>
        {
            new()
            {
                Id = 1,
                Spell = "Resource",
                Value = 232324,
                ResourcePerSecond = 3421,
                CastNumber = 34,
                MinValue = 100,
                MaxValue = 1230,
                AverageValue = 145,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 2,
                Spell = "Resource",
                Value = 232324,
                ResourcePerSecond = 2421,
                CastNumber = 45,
                MinValue = 120,
                MaxValue = 6230,
                AverageValue = 937,
                CombatPlayerId = 1,
            },
            new()
            {
                Id = 3,
                Spell = "Resource",
                Value = 232324,
                ResourcePerSecond = 10421,
                CastNumber = 24,
                MinValue = 1300,
                MaxValue = 11230,
                AverageValue = 1450,
                CombatPlayerId = 1,
            }
        };

        return collection;
    }
}
