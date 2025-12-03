using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class UserPostCommentTestDataFactory
{
    public static UserPostComment Create(int id = 1, int userPostId = 1)
    {
        var entity = new UserPostComment
        {
            Id = id,
            Content = "test",
            UserPostId = userPostId,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static UserPostCommentDto CreateDto(int id = 1, int userPostId = 1)
    {
        var entityDto = new UserPostCommentDto
        {
            Id = id,
            Content = "test",
            UserPostId = userPostId,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<UserPostComment> CreateCollection()
    {
        var collection = new List<UserPostComment>
        {
            new () {
                Id = 1,
                Content = "test",
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Content = "test 1",
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                Content = "test 2",
                UserPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }

    public static List<UserPostCommentDto> CreateDtoCollection()
    {
        var collection = new List<UserPostCommentDto>
        {
            new () {
                Id = 1,
                Content = "test",
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Content = "test 1",
                UserPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                Content = "test 2",
                UserPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }
}
