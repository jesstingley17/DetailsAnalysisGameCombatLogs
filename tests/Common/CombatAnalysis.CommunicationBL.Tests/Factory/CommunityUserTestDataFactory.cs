using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityUserTestDataFactory
{
    public static CommunityUser Create(string id = "uid-1", int communityId = 1)
    {
        var entity = new CommunityUser
        {
            Id = id,
            Username = "Solinx",
            AppUserId = "uid-1-1",
            CommunityId = communityId,
        };

        return entity;
    }

    public static CommunityUserDto CreateDto(string id = "uid-1", int communityId = 1)
    {
        var entityDto = new CommunityUserDto(Id: id,
            Username: "Solinx",
            AppUserId: "uid-1-1",
            CommunityId: communityId
        );

        return entityDto;
    }

    public static List<CommunityUser> CreateCollection()
    {
        var collection = new List<CommunityUser>
        {
            new () {
                Id = "uid-1",
                Username = "Solinx",
                AppUserId = "uid-1-1",
                CommunityId = 1,
            },
            new () {
                Id = "uid-2",
                Username = "Solinx",
                AppUserId = "uid-1-2",
                CommunityId = 1,
            },
            new () {
                Id = "uid-3",
                Username = "Solinx",
                AppUserId = "uid-1-3",
                CommunityId = 1,
            }
        };

        return collection;
    }

    public static List<CommunityUserDto> CreateDtoCollection()
    {
        var collection = new List<CommunityUserDto>
        {
            new (Id: "uid-1",
                Username: "Solinx",
                AppUserId: "uid-1-1",
                CommunityId: 1
            ),
            new (Id: "uid-2",
                Username: "Solinx",
                AppUserId: "uid-1-2",
                CommunityId: 1
            ),
            new (Id: "uid-3",
                Username: "Solinx",
                AppUserId: "uid-1-3",
                CommunityId: 1
            ),
        };

        return collection;
    }
}
