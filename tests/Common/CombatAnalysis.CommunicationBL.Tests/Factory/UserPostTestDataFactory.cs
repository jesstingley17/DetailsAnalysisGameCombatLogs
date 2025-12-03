using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class UserPostTestDataFactory
{
    public static UserPost Create(int id = 1, string content = "Com name")
    {
        var entity = new UserPost
        {
            Id = id,
            Owner = "Solinx",
            Content = content,
            PublicType = 0,
            Tags = "tage",
            CreatedAt = DateTimeOffset.UtcNow,
            LikeCount = 0,
            DislikeCount = 0,
            CommentCount = 0,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static UserPostDto CreateDto(int id = 1, string content = "Com name")
    {
        var entityDto = new UserPostDto
        {
            Id = id,
            Owner = "Solinx",
            Content = content,
            PublicType = 0,
            Tags = "tage",
            CreatedAt = DateTimeOffset.UtcNow,
            LikeCount = 0,
            DislikeCount = 0,
            CommentCount = 0,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<UserPost> CreateCollection()
    {
        var collection = new List<UserPost>
        {
            new () {
                Id = 1,
                Owner = "Solinx",
                Content = "check",
                PublicType = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Owner = "Solinx",
                Content = "check",
                PublicType = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                Owner = "Solinx",
                Content = "check",
                PublicType = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }

    public static List<UserPostDto> CreateDtoCollection()
    {
        var collection = new List<UserPostDto>
        {
            new () {
                Id = 1,
                Owner = "Solinx",
                Content = "check",
                PublicType = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Owner = "Solinx",
                Content = "check",
                PublicType = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                Owner = "Solinx",
                Content = "check",
                PublicType = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }
}
