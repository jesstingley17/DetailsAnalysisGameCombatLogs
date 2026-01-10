using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class CombatTestDataFactory
{
    public static Combat Create(int id = 1, int difficulty = 0)
    {
        var entity = new Combat
        {
            Id = id,
            DungeonName = "Test",
            DamageDone = 10000,
            HealDone = 3000,
            DamageTaken = 110,
            ResourcesRecovery = 10,
            IsWin = true,
            StartDate = DateTime.Now.AddHours(1),
            FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
            IsReady = true,
            CombatLogId = 1
        };

        return entity;
    }

    public static CombatDto CreateDto(int id = 1, string dungeonName = "dungeon")
    {
        var entityDto = new CombatDto
        {
            Id = id,
            DungeonName = dungeonName,
            DamageDone = 10000,
            HealDone = 3000,
            DamageTaken = 110,
            ResourcesRecovery = 10,
            IsWin = true,
            StartDate = DateTime.Now.AddHours(1),
            FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
            IsReady = true,
            CombatLogId = 1
        };

        return entityDto;
    }

    public static List<Combat> CreateCollection()
    {
        var collection = new List<Combat>
        {
            new () {
                Id = 1,
                DungeonName = "Test",
                DamageDone = 10000,
                HealDone = 3000,
                DamageTaken = 110,
                ResourcesRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 2,
                DungeonName = "Test",
                DamageDone = 1500,
                HealDone = 30000,
                DamageTaken = 210,
                ResourcesRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(2),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 3,
                DungeonName = "Test",
                DamageDone = 25000,
                HealDone = 114000,
                DamageTaken = 2500,
                ResourcesRecovery = 1005,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(4),
                IsReady = true,
                CombatLogId = 1
            }
        };

        return collection;
    }

    public static List<CombatDto> CreateDtoCollection()
    {
        var collection = new List<CombatDto>
        {
            new () {
                Id = 1,
                DungeonName = "Test",
                DamageDone = 10000,
                HealDone = 3000,
                DamageTaken = 110,
                ResourcesRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 2,
                DungeonName = "Test",
                DamageDone = 1500,
                HealDone = 30000,
                DamageTaken = 210,
                ResourcesRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(2),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 3,
                DungeonName = "Test",
                DamageDone = 25000,
                HealDone = 114000,
                DamageTaken = 2500,
                ResourcesRecovery = 1005,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(4),
                IsReady = true,
                CombatLogId = 1
            }
        };

        return collection;
    }
}
