using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityDiscussionTestDataFactory
{
    public static CommunityDiscussion Create(int id = 1, int communityId = 1)
    {
        var entity = new CommunityDiscussion
        {
            Id = id,
            Title = "title",
            Content = "content",
            When = DateTimeOffset.UtcNow,
            AppUserId = "uid-1-1",
            CommunityId = communityId
        };

        return entity;
    }

    public static CommunityDiscussionDto CreateDto(int id = 1, int communityId = 1)
    {
        var entityDto = new CommunityDiscussionDto(Id: id,
            Title: "title",
            Content: "content",
            When: DateTimeOffset.UtcNow,
            AppUserId: "uid-1-1",
            CommunityId: communityId);

        return entityDto;
    }

    public static List<CommunityDiscussion> CreateCollection()
    {
        var collection = new List<CommunityDiscussion>
        {
            new () {
                Id = 1,
                Title = "title",
                Content = "content",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
                CommunityId = 1
            },
            new () {
                Id = 2,
                Title = "title 1",
                Content = "content",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
                CommunityId = 1
            },
            new () {
                Id = 3,
                Title = "title 2",
                Content = "content",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
                CommunityId = 1
            }
        };

        return collection;
    }

    public static List<CommunityDiscussionDto> CreateDtoCollection()
    {
        var collection = new List<CommunityDiscussionDto>
        {
            new (Id: 1,
                Title: "title",
                Content: "content",
                When: DateTimeOffset.UtcNow,
                AppUserId: "uid-1-1",
                CommunityId: 1
            ),
            new (Id: 2,
                Title: "title 1",
                Content: "content",
                When: DateTimeOffset.UtcNow,
                AppUserId: "uid-1-1",
                CommunityId: 1
            ),
            new (Id: 3,
                Title: "title 2",
                Content: "content",
                When: DateTimeOffset.UtcNow,
                AppUserId: "uid-1-1",
                CommunityId: 1
            ),
        };

        return collection;
    }
}
