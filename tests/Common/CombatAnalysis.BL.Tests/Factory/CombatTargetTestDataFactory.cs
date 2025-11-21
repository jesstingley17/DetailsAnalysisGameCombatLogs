using CombatAnalysis.BL.DTO;
using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.BL.Tests.Factory;

internal class CombatTargetTestDataFactory
{
    public static CombatTarget Create(int id = 1, string username = "Solinx")
    {
        var entity = new CombatTarget
        {
            Id = id,
            Username = username,
            Target = "Boss",
            Sum = 213400
        };

        return entity;
    }

    public static CombatTargetDto CreateDto(int id = 1, string username = "Solinx")
    {
        var entityDto = new CombatTargetDto
        {
            Id = id,
            Username = username,
            Target = "Boss",
            Sum = 213400
        };

        return entityDto;
    }

    public static List<CombatTarget> CreateCollection()
    {
        var collection = new List<CombatTarget>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                Target = "Boss",
                Sum = 213400
            },
            new () {
                Id = 2,
                Username = "Solinx",
                Target = "Boss",
                Sum = 113400
            },
            new () {
                Id = 3,
                Username = "Solinx",
                Target = "Boss",
                Sum = 313400
            }
        };

        return collection;
    }

    public static List<CombatTargetDto> CreateDtoColelction()
    {
        var collection = new List<CombatTargetDto>
        {
            new () {
                Id = 1,
                Username = "Solinx",
                Target = "Boss",
                Sum = 213400
            },
            new () {
                Id = 2,
                Username = "Solinx",
                Target = "Boss",
                Sum = 113400
            },
            new () {
                Id = 3,
                Username = "Solinx",
                Target = "Boss",
                Sum = 313400
            }
        };

        return collection;
    }
}
