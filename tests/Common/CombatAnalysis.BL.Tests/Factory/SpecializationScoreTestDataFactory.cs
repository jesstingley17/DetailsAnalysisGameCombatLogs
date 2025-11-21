using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class SpecializationScoreTestDataFactory
{
    public static SpecializationScore Create(int id = 1, int difficult = 0)
    {
        var entity = new SpecializationScore
        {
            Id = id,
            SpecId = 1,
            BossId = 1,
            Difficult = difficult,
            Damage = 30345,
            Heal = 23412,
            Updated = DateTimeOffset.UtcNow,
        };

        return entity;
    }

    public static SpecializationScoreDto CreateDto(int id = 1, int difficult = 0)
    {
        var entityDto = new SpecializationScoreDto
        {
            Id = id,
            SpecId = 1,
            BossId = 1,
            Difficult = difficult,
            Damage = 30345,
            Heal = 23412,
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
                SpecId = 1,
                BossId = 1,
                Difficult = 0,
                Damage = 30345,
                Heal = 23412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 2,
                SpecId = 1,
                BossId = 2,
                Difficult = 0,
                Damage = 20345,
                Heal = 33412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 3,
                SpecId = 1,
                BossId = 3,
                Difficult = 0,
                Damage = 31345,
                Heal = 23562,
                Updated = DateTimeOffset.UtcNow,
            }
        };

        return collection;
    }

    public static List<SpecializationScoreDto> CreateDtoColelction()
    {
        var collection = new List<SpecializationScoreDto>
        {
            new () {
                Id = 1,
                SpecId = 1,
                BossId = 1,
                Difficult = 0,
                Damage = 30345,
                Heal = 23412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 2,
                SpecId = 1,
                BossId = 2,
                Difficult = 0,
                Damage = 20345,
                Heal = 33412,
                Updated = DateTimeOffset.UtcNow,
            },
            new () {
                Id = 3,
                SpecId = 1,
                BossId = 3,
                Difficult = 0,
                Damage = 31345,
                Heal = 23562,
                Updated = DateTimeOffset.UtcNow,
            }
        };

        return collection;
    }
}
