using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class CombatPlayerPositionTestDataFactory
{
    public static CombatPlayerPosition Create(int id = 1, double positionX = 22, double positionY = 34)
    {
        var entity = new CombatPlayerPosition
        {
            Id = id,
            PositionX = positionX,
            PositionY = positionY,
            Time = TimeSpan.Parse("00:01:11"),
            CombatPlayerId = 1,
            CombatId = 1,
        };

        return entity;
    }

    public static CombatPlayerPositionDto CreateDto(int id = 1, double positionX = 22, double positionY = 34)
    {
        var entityDto = new CombatPlayerPositionDto
        {
            Id = id,
            PositionX = positionX,
            PositionY = positionY,
            Time = TimeSpan.Parse("00:01:11"),
            CombatPlayerId = 1,
            CombatId = 1,
        };

        return entityDto;
    }

    public static List<CombatPlayerPosition> CreateCollection()
    {
        var collection = new List<CombatPlayerPosition>
        {
            new () {
                Id = 1,
                PositionX = 23,
                PositionY = 34,
                Time = TimeSpan.Parse("00:01:11"),
                CombatPlayerId = 1,
                CombatId = 1,
            },
            new () {
                Id = 2,
                PositionX = 13,
                PositionY = 14,
                Time = TimeSpan.Parse("00:01:23"),
                CombatPlayerId = 1,
                CombatId = 1,
            },
            new () {
                Id = 3,
                PositionX = 25,
                PositionY = 54,
                Time = TimeSpan.Parse("00:01:45"),
                CombatPlayerId = 1,
                CombatId = 1,
            }
        };

        return collection;
    }

    public static List<CombatPlayerPositionDto> CreateDtoCollection()
    {
        var collection = new List<CombatPlayerPositionDto>
        {   
            new () {
                Id = 1,
                PositionX = 23,
                PositionY = 34,
                Time = TimeSpan.Parse("00:01:11"),
                CombatPlayerId = 1,
                CombatId = 1,
            },
            new () {
                Id = 2,
                PositionX = 13,
                PositionY = 14,
                Time = TimeSpan.Parse("00:01:23"),
                CombatPlayerId = 1,
                CombatId = 1,
            },
            new () {
                Id = 3,
                PositionX = 25,
                PositionY = 54,
                Time = TimeSpan.Parse("00:01:45"),
                CombatPlayerId = 1,
                CombatId = 1,
            }
        };

        return collection;
    }
}
