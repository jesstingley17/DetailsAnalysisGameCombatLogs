using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationBL.Tests.Factory;

internal class CommunityTestDataFactory
{
    public static Community Create(int id = 1, string name = "Com name")
    {
        var entity = new Community
        {
            Id = id,
            Name = name,
            Description = "com des c",
            PolicyType = 0,
            AppUserId = "uid-1-1",
        };

        return entity;
    }

    public static CommunityDto CreateDto(int id = 1, string name = "Com name")
    {
        var entityDto = new CommunityDto(Id: id,
            Name: name,
            Description: "com des c",
            PolicyType: Enums.CommunityPolicyType.Public,
            AppUserId: "uid-1-1"
        );

        return entityDto;
    }

    public static List<Community> CreateCollection()
    {
        var collection = new List<Community>
        {
            new () {
                Id = 1,
                Name = "name",
                Description = "com des c",
                PolicyType = 0,
                AppUserId = "uid-1-1",
            },
            new () {
                Id = 2,
                Name = "name 1",
                Description = "com des c",
                PolicyType = 0,
                AppUserId = "uid-1-2",
            },
            new () {
                Id = 3,
                Name = "name 2",
                Description = "com des c",
                PolicyType = 0,
                AppUserId = "uid-1-3",
            }
        };

        return collection;
    }

    public static List<CommunityDto> CreateDtoCollection()
    {
        var collection = new List<CommunityDto>
        {
            new (Id: 1,
                Name: "name",
                Description: "com des c",
                PolicyType: Enums.CommunityPolicyType.Public,
                AppUserId: "uid-1-1"
            ),
            new (Id: 2,
                Name: "name 1",
                Description: "com des c",
                PolicyType: Enums.CommunityPolicyType.Public,
                AppUserId: "uid-1-2"
            ),
            new (Id: 3,
                Name: "name 2",
                Description: "com des c",
                PolicyType: Enums.CommunityPolicyType.Public,
                AppUserId: "uid-1-3"
            ),
        };

        return collection;
    }
}
