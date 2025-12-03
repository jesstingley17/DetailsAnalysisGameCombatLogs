using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityDiscussionCommentTestDataFactory
{
    public static CommunityDiscussionComment Create(int id = 1, int communityCommentId = 1)
    {
        var entity = new CommunityDiscussionComment
        {
            Id = id,
            Content = "content",
            When = DateTimeOffset.UtcNow,
            AppUserId = "uid-1-1",
            CommunityDiscussionId = communityCommentId
        };

        return entity;
    }

    public static CommunityDiscussionCommentDto CreateDto(int id = 1, int communityCommentId = 1)
    {
        var entityDto = new CommunityDiscussionCommentDto(Id: id,
            Content: "content",
            When: DateTimeOffset.UtcNow,
            AppUserId: "uid-1-1",
            CommunityDiscussionId: communityCommentId);

        return entityDto;
    }

    public static List<CommunityDiscussionComment> CreateCollection()
    {
        var collection = new List<CommunityDiscussionComment>
        {
            new () {
                Id = 1,
                Content = "content",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
                CommunityDiscussionId = 1
            },
            new () {
                Id = 2,
                Content = "content",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
                CommunityDiscussionId = 1
            },
            new () {
                Id = 3,
                Content = "content",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
                CommunityDiscussionId = 1
            }
        };

        return collection;
    }

    public static List<CommunityDiscussionCommentDto> CreateDtoCollection()
    {
        var collection = new List<CommunityDiscussionCommentDto>
        {
            new (Id: 1,
                Content: "content",
                When: DateTimeOffset.UtcNow,
                AppUserId: "uid-1-1",
                CommunityDiscussionId: 1
            ),
            new (Id: 2,
                Content: "content",
                When: DateTimeOffset.UtcNow,
                AppUserId: "uid-1-1",
                CommunityDiscussionId: 1
            ),
            new (Id: 3,
                Content: "content",
                When: DateTimeOffset.UtcNow,
                AppUserId: "uid-1-1",
                CommunityDiscussionId: 1
            ),
        };

        return collection;
    }
}
