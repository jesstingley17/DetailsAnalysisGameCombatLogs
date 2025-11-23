using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class CombatPlayerTestDataFactory
{
    public static CombatPlayer Create(int id = 1, string username = "Solinx")
    {
        var entity = new CombatPlayer
        {
            Id = id,
            Username = username,
            PlayerId = "uid-234",
            AverageItemLevel = 345,
            ResourcesRecovery = 3452,
            DamageDone = 10000,
            HealDone = 3000,
            DamageTaken = 110,
            CombatId = 1
        };

        return entity;
    }

    public static CombatPlayerDto CreateDto(int id = 1, string username = "Solinx")
    {
        var entityDto = new CombatPlayerDto
        {
            Id = id,
            Username = username,
            PlayerId = "uid-234",
            AverageItemLevel = 345,
            ResourcesRecovery = 3452,
            DamageDone = 10000,
            HealDone = 3000,
            DamageTaken = 110,
            CombatId = 1
        };

        return entityDto;
    }

    public static List<CombatPlayer> CreateCollection()
    {
        var collection = new List<CombatPlayer>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                PlayerId = "uid-234",
                AverageItemLevel = 345,
                ResourcesRecovery = 3452,
                DamageDone = 10000,
                HealDone = 3000,
                DamageTaken = 990,
                CombatId = 1
            },
            new () {
                Id = 2,
                Username = "Solinx",
                PlayerId = "uid-234",
                AverageItemLevel = 341,
                ResourcesRecovery = 10452,
                DamageDone = 12000,
                HealDone = 4000,
                DamageTaken = 900,
                CombatId = 1
            },
            new () {
                Id = 3,
                Username = "Solinx",
                PlayerId = "uid-234",
                AverageItemLevel = 339,
                ResourcesRecovery = 2452,
                DamageDone = 9000,
                HealDone = 3200,
                DamageTaken = 1200,
                CombatId = 1
            }
        };

        return collection;
    }

    public static List<CombatPlayerDto> CreateDtoCollection()
    {
        var collection = new List<CombatPlayerDto>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                PlayerId = "uid-234",
                AverageItemLevel = 345,
                ResourcesRecovery = 3452,
                DamageDone = 10000,
                HealDone = 3000,
                DamageTaken = 990,
                CombatId = 1
            },
            new () {
                Id = 2,
                Username = "Solinx",
                PlayerId = "uid-234",
                AverageItemLevel = 341,
                ResourcesRecovery = 10452,
                DamageDone = 12000,
                HealDone = 4000,
                DamageTaken = 900,
                CombatId = 1
            },
            new () {
                Id = 3,
                Username = "Solinx",
                PlayerId = "uid-234",
                AverageItemLevel = 339,
                ResourcesRecovery = 2452,
                DamageDone = 9000,
                HealDone = 3200,
                DamageTaken = 1200,
                CombatId = 1
            }
        };

        return collection;
    }
}
