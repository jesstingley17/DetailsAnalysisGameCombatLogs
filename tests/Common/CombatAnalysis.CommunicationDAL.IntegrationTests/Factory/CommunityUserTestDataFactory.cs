using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.Factory;

internal static class CommunityUserTestDataFactory
{
    public static CommunityUser Create(string id = "uid-1", string username = "Solinx")
    {
        var collection = new CommunityUser
        {
            Id = id,
            Username = username,
            AppUserId = "uid-1-1",
            CommunityId = 1,
        };

        return collection;
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
}
