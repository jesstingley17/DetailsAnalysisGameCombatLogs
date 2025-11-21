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
            LocallyNumber = 1,
            DungeonName = "Test",
            Name = "Boss name",
            Difficulty = difficulty,
            DamageDone = 10000,
            HealDone = 3000,
            DamageTaken = 110,
            EnergyRecovery = 10,
            IsWin = true,
            StartDate = DateTime.Now.AddHours(1),
            FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
            IsReady = true,
            CombatLogId = 1
        };

        return entity;
    }

    public static CombatDto CreateDto(int id = 1, int difficulty = 0)
    {
        var entityDto = new CombatDto
        {
            Id = id,
            LocallyNumber = 1,
            DungeonName = "Test",
            Name = "Boss name",
            Difficulty = difficulty,
            DamageDone = 10000,
            HealDone = 3000,
            DamageTaken = 110,
            EnergyRecovery = 10,
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
                LocallyNumber = 1,
                DungeonName = "Test",
                Name = "Boss name",
                Difficulty = 0,
                DamageDone = 10000,
                HealDone = 3000,
                DamageTaken = 110,
                EnergyRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 2,
                LocallyNumber = 1,
                DungeonName = "Test",
                Name = "Boss name 1",
                Difficulty = 0,
                DamageDone = 1500,
                HealDone = 30000,
                DamageTaken = 210,
                EnergyRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(2),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 3,
                LocallyNumber = 1,
                DungeonName = "Test",
                Name = "Boss name 2",
                Difficulty = 0,
                DamageDone = 25000,
                HealDone = 114000,
                DamageTaken = 2500,
                EnergyRecovery = 1005,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(4),
                IsReady = true,
                CombatLogId = 1
            }
        };

        return collection;
    }

    public static List<CombatDto> CreateDtoColelction()
    {
        var collection = new List<CombatDto>
        {
            new () {
                Id = 1,
                LocallyNumber = 1,
                DungeonName = "Test",
                Name = "Boss name",
                Difficulty = 0,
                DamageDone = 10000,
                HealDone = 3000,
                DamageTaken = 110,
                EnergyRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(3),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 2,
                LocallyNumber = 1,
                DungeonName = "Test",
                Name = "Boss name 1",
                Difficulty = 0,
                DamageDone = 1500,
                HealDone = 30000,
                DamageTaken = 210,
                EnergyRecovery = 10,
                IsWin = true,
                StartDate = DateTime.Now.AddHours(1),
                FinishDate = DateTime.Now.AddHours(1).AddMinutes(2),
                IsReady = true,
                CombatLogId = 1
            },
            new () {
                Id = 3,
                LocallyNumber = 1,
                DungeonName = "Test",
                Name = "Boss name 2",
                Difficulty = 0,
                DamageDone = 25000,
                HealDone = 114000,
                DamageTaken = 2500,
                EnergyRecovery = 1005,
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
