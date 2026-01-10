using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class SpecializationScoreTestDataFactory
{
    public static SpecializationScore Create(int id = 1, int damage = 0)
    {
        var entity = new SpecializationScore
        {
            Id = id,
            SpecializationId = 1,
            DamageDone = damage,
            HealDone = 23412,
            Updated = DateTimeOffset.UtcNow,
        };

        return entity;
    }

    public static SpecializationScoreDto CreateDto(int id = 1, int damage = 0)
    {
        var entityDto = new SpecializationScoreDto
        {
            Id = id,
            SpecializationId = 1,
            DamageDone = damage,
            HealDone = 23412,
            Updated = DateTimeOffset.UtcNow,
        };

        return entityDto;
    }

    public static List<SpecializationScore> CreateCollection()
    {
        var collection = new List<SpecializationScore>
        {
            new () {
                Id = 1,
                SpecializationId = 1,
                DamageDone = 30345,
                HealDone = 23412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 2,
                SpecializationId = 1,
                DamageDone = 20345,
                HealDone = 33412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 3,
                SpecializationId = 1,
                DamageDone = 31345,
                HealDone = 23562,
                Updated = DateTimeOffset.UtcNow,
            }
        };

        return collection;
    }

    public static List<SpecializationScoreDto> CreateDtoCollection()
    {
        var collection = new List<SpecializationScoreDto>
        {
            new () {
                Id = 1,
                SpecializationId = 1,
                DamageDone = 30345,
                HealDone = 23412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 2,
                SpecializationId = 1,
                DamageDone = 20345,
                HealDone = 33412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 3,
                SpecializationId = 1,
                DamageDone = 31345,
                HealDone = 23562,
                Updated = DateTimeOffset.UtcNow,
            }
        };

        return collection;
    }
}
