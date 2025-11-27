using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class UserPostDislikeTestDataFactory
{
    public static UserPostDislike Create(int id = 1, int userPostId = 1)
    {
        var entity = new UserPostDislike
        {
            Id = id,
            UserPostId = userPostId,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static UserPostDislikeDto CreateDto(int id = 1, int userPostId = 1)
    {
        var entityDto = new UserPostDislikeDto
        {
            Id = id,
            UserPostId = userPostId,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<UserPostDislike> CreateCollection()
    {
        var collection = new List<UserPostDislike>
        {
            new () {
                Id = 1,
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                UserPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }

    public static List<UserPostDislikeDto> CreateDtoCollection()
    {
        var collection = new List<UserPostDislikeDto>
        {
            new () {
                Id = 1,
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                UserPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }
}
