using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class PlayerParseInfoTestDataFactory
{
    public static PlayerParseInfo Create(int id = 1, int specId = 1)
    {
        var entity = new PlayerParseInfo
        {
            Id = id,
            SpecId = specId,
            ClassId = 1,
            BossId = 1,
            Difficult = 1,
            DamageEfficiency = 23132,
            HealEfficiency = 23134,
            CombatPlayerId = 1
        };

        return entity;
    }

    public static PlayerParseInfoDto CreateDto(int id = 1, int specId = 1)
    {
        var entityDto = new PlayerParseInfoDto
        {
            Id = id,
            SpecId = specId,
            ClassId = 1,
            BossId = 1,
            Difficult = 1,
            DamageEfficiency = 23132,
            HealEfficiency = 23134,
            CombatPlayerId = 1
        };

        return entityDto;
    }

    public static List<PlayerParseInfo> CreateCollection()
    {
        var collection = new List<PlayerParseInfo>
        {
            new () {
                Id = 1,
                SpecId = 1,
                ClassId = 1,
                BossId = 1,
                Difficult = 1,
                DamageEfficiency = 23132,
                HealEfficiency = 23134,
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                SpecId = 1,
                ClassId = 1,
                BossId = 2,
                Difficult = 1,
                DamageEfficiency = 13132,
                HealEfficiency = 21134,
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                SpecId = 1,
                ClassId = 1,
                BossId = 3,
                Difficult = 1,
                DamageEfficiency = 43132,
                HealEfficiency = 23234,
                CombatPlayerId = 1
            }
        };

        return collection;
    }

    public static List<PlayerParseInfoDto> CreateDtoColelction()
    {
        var collection = new List<PlayerParseInfoDto>
        {
            new () {
                Id = 1,
                SpecId = 1,
                ClassId = 1,
                BossId = 1,
                Difficult = 1,
                DamageEfficiency = 23132,
                HealEfficiency = 23134,
                CombatPlayerId = 1
            },
            new () {
                Id = 2,
                SpecId = 1,
                ClassId = 1,
                BossId = 2,
                Difficult = 1,
                DamageEfficiency = 13132,
                HealEfficiency = 21134,
                CombatPlayerId = 1
            },
            new () {
                Id = 3,
                SpecId = 1,
                ClassId = 1,
                BossId = 3,
                Difficult = 1,
                DamageEfficiency = 43132,
                HealEfficiency = 23234,
                CombatPlayerId = 1
            }
        };

        return collection;
    }
}
