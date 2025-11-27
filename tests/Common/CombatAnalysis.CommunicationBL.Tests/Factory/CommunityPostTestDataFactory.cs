using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityPostTestDataFactory
{
    public static CommunityPost Create(int id = 1, string name = "Com name")
    {
        var entity = new CommunityPost
        {
            Id = id,
            CommunityName = name,
            Owner = "Solinx",
            Content = "check",
            PostType = 0,
            PublicType = 0,
            Restrictions = 0,
            Tags = "tage",
            CreatedAt = DateTimeOffset.UtcNow,
            LikeCount = 0,
            DislikeCount = 0,
            CommentCount = 0,
            AppUserId = "uid-1-1",
            CommunityId = 1
        };

        return entity;
    }

    public static CommunityPostDto CreateDto(int id = 1, string name = "Com name")
    {
        var entityDto = new CommunityPostDto
        {
            Id = id,
            CommunityName = name,
            Owner = "Solinx",
            Content = "check",
            PostType = 0,
            PublicType = 0,
            Restrictions = 0,
            Tags = "tage",
            CreatedAt = DateTimeOffset.UtcNow,
            LikeCount = 0,
            DislikeCount = 0,
            CommentCount = 0,
            AppUserId = "uid-1-1",
            CommunityId = 1
        };

        return entityDto;
    }

    public static List<CommunityPost> CreateCollection()
    {
        var collection = new List<CommunityPost>
        {
            new () {
                Id = 1,
                CommunityName = "check",
                Owner = "Solinx",
                Content = "check",
                PostType = 0,
                PublicType = 0,
                Restrictions = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
                CommunityId = 1
            },
            new () {
                Id = 2,
                CommunityName = "check 1",
                Owner = "Solinx",
                Content = "check",
                PostType = 0,
                PublicType = 0,
                Restrictions = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
                CommunityId = 1
            },
            new () {
                Id = 3,
                CommunityName = "check 2",
                Owner = "Solinx",
                Content = "check",
                PostType = 0,
                PublicType = 0,
                Restrictions = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
                CommunityId = 1
            }
        };

        return collection;
    }

    public static List<CommunityPostDto> CreateDtoCollection()
    {
        var collection = new List<CommunityPostDto>
        {
            new () {
                Id = 1,
                CommunityName = "check",
                Owner = "Solinx",
                Content = "check",
                PostType = 0,
                PublicType = 0,
                Restrictions = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
                CommunityId = 1
            },
            new () {
                Id = 2,
                CommunityName = "check 1",
                Owner = "Solinx",
                Content = "check",
                PostType = 0,
                PublicType = 0,
                Restrictions = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
                CommunityId = 1
            },
            new () {
                Id = 3,
                CommunityName = "check 2",
                Owner = "Solinx",
                Content = "check",
                PostType = 0,
                PublicType = 0,
                Restrictions = 0,
                Tags = "tage",
                CreatedAt = DateTimeOffset.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                AppUserId = "uid-1-1",
                CommunityId = 1
            }
        };

        return collection;
    }
}
