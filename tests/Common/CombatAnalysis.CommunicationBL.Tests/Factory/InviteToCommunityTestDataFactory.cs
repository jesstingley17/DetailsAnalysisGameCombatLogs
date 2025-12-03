using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class InviteToCommunityTestDataFactory
{
    public static InviteToCommunity Create(int id = 1, int communityId = 1)
    {
        var entity = new InviteToCommunity
        {
            Id = id,
            CommunityId = communityId,
            ToAppUserId = "uid-1",
            When = DateTimeOffset.UtcNow,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static InviteToCommunityDto CreateDto(int id = 1, int communityId = 1)
    {
        var entityDto = new InviteToCommunityDto(Id: id,
            CommunityId: communityId,
            ToAppUserId: "uid-1",
            When: DateTimeOffset.UtcNow,
            AppUserId: "uid-1-1"
        );

        return entityDto;
    }

    public static List<InviteToCommunity> CreateCollection()
    {
        var collection = new List<InviteToCommunity>
        {
            new () {
                Id = 1,
                CommunityId = 1,
                ToAppUserId = "uid-1",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                CommunityId = 1,
                ToAppUserId = "uid-2",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-2",
            },
            new () {
                Id = 3,
                CommunityId = 1,
                ToAppUserId = "uid-3",
                When = DateTimeOffset.UtcNow,
                AppUserId = "uid-1-3",
            }
        };

        return collection;
    }

    public static List<InviteToCommunityDto> CreateDtoCollection()
    {
        var collection = new List<InviteToCommunityDto>
        {
            new (Id: 1,
                CommunityId: 1,
                ToAppUserId: "Test",
                When: DateTimeOffset.UtcNow,
                AppUserId: "utc-1"
            ),
            new (Id: 2,
                CommunityId: 1,
                ToAppUserId: "Test",
                When: DateTimeOffset.UtcNow,
                AppUserId: "utc-1"
            ),
            new (Id: 3,
                CommunityId: 1,
                ToAppUserId: "Test",
                When: DateTimeOffset.UtcNow,
                AppUserId: "utc-1"
            ),
        };

        return collection;
    }
}
