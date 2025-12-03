using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityPostCommentTestDataFactory
{
    public static CommunityPostComment Create(int id = 1, int communityId = 1)
    {
        var entity = new CommunityPostComment
        {
            Id = id,
            Content = "content",
            CommentType = 0,
            CommunityId = communityId,
            CommunityPostId = 1,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static CommunityPostCommentDto CreateDto(int id = 1, int communityId = 1)
    {
        var entityDto = new CommunityPostCommentDto
        {
            Id = id,
            Content = "content",
            CommentType = 0,
            CommunityId = communityId,
            CommunityPostId = 1,
            AppUserId = "uid-1-1",
        };

        return entityDto;
    }

    public static List<CommunityPostComment> CreateCollection()
    {
        var collection = new List<CommunityPostComment>
        {
            new () {
                Id = 1,
                Content = "content",
                CommentType = 0,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Content = "content 1",
                CommentType = 0,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                Content = "content 2",
                CommentType = 0,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }

    public static List<CommunityPostCommentDto> CreateDtoCollection()
    {
        var collection = new List<CommunityPostCommentDto>
        {
            new () {
                Id = 1,
                Content = "content",
                CommentType = 0,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Content = "content 1",
                CommentType = 0,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 3,
                Content = "content 2",
                CommentType = 0,
                CommunityId = 1,
                CommunityPostId = 1,
                AppUserId = "uid-1-1",
            }
        };

        return collection;
    }
}
