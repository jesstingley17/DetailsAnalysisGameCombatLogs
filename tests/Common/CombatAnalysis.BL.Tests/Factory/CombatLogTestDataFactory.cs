using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class CombatLogTestDataFactory
{
    public static CombatLog Create(int id = 1, string name = "Dung name")
    {
        var entity = new CombatLog
        {
            Id = id,
            Name = name,
            Date = DateTimeOffset.UtcNow,
            LogType = 0,
            NumberReadyCombats = 3,
            CombatsInQueue = 0,
            IsReady = true,
            AppUserId = "uid-22",
        };

        return entity;
    }

    public static CombatLogDto CreateDto(int id = 1, string name = "Dung name")
    {
        var entityDto = new CombatLogDto
        {
            Id = id,
            Name = name,
            Date = DateTimeOffset.UtcNow,
            LogType = 0,
            NumberReadyCombats = 3,
            CombatsInQueue = 0,
            IsReady = true,
            AppUserId = "uid-22",
        };

        return entityDto;
    }

    public static List<CombatLog> CreateCollection()
    {
        var collection = new List<CombatLog>
        {
            new () {
                Id = 1,
                Name = "Dung name",
                Date = DateTimeOffset.UtcNow,
                LogType = 0,
                NumberReadyCombats = 3,
                CombatsInQueue = 0,
                IsReady = true,
                AppUserId = "uid-22",
            },
            new () {
                Id = 2,
                Name = "Dung name",
                Date = DateTimeOffset.UtcNow,
                LogType = 0,
                NumberReadyCombats = 1,
                CombatsInQueue = 2,
                IsReady = false,
                AppUserId = "uid-22",
            },
            new () {
                Id = 3,
                Name = "Dung name",
                Date = DateTimeOffset.UtcNow,
                LogType = 0,
                NumberReadyCombats = 0,
                CombatsInQueue = 3,
                IsReady = true,
                AppUserId = "uid-22",
            }
        };

        return collection;
    }

    public static List<CombatLogDto> CreateDtoColelction()
    {
        var collection = new List<CombatLogDto>
        {
            new () {
                Id = 1,
                Name = "Dung name",
                Date = DateTimeOffset.UtcNow,
                LogType = 0,
                NumberReadyCombats = 3,
                CombatsInQueue = 0,
                IsReady = true,
                AppUserId = "uid-22",
            },
            new () {
                Id = 2,
                Name = "Dung name",
                Date = DateTimeOffset.UtcNow,
                LogType = 0,
                NumberReadyCombats = 1,
                CombatsInQueue = 2,
                IsReady = false,
                AppUserId = "uid-22",
            },
            new () {
                Id = 3,
                Name = "Dung name",
                Date = DateTimeOffset.UtcNow,
                LogType = 0,
                NumberReadyCombats = 0,
                CombatsInQueue = 3,
                IsReady = true,
                AppUserId = "uid-22",
            }
        };

        return collection;
    }
}
