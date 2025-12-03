using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class UserPostLikeTestDataFactory
{
    public static UserPostLike Create(int id = 1, int userPostId = 1)
    {
        var entity = new UserPostLike
        {
            Id = id,
            UserPostId = userPostId,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static UserPostLikeDto CreateDto(int id = 1, int userPostId = 1)
    {
        var entityDto = new UserPostLikeDto
        {
            Id = id,
            UserPostId = userPostId,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<UserPostLike> CreateCollection()
    {
        var collection = new List<UserPostLike>
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

    public static List<UserPostLikeDto> CreateDtoCollection()
    {
        var collection = new List<UserPostLikeDto>
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
