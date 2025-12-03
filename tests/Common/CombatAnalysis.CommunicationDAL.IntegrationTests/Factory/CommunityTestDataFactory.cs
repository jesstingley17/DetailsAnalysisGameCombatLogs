using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.Factory;

internal static class CommunityTestDataFactory
{
    public static Community Create(int id = 1, string name = "Com test")
    {
        var collection = new Community
        {
            Id = id,
            Name = name,
            Description = "test description",
            PolicyType = 0,
            AppUserId = "uid-1"
        };

        return collection;
    }

    public static List<Community> CreateCollection()
    {
        var collection = new List<Community>
        {
            new () {
                Id = 1,
                Name = "Test",
                Description = "test description",
                PolicyType = 0,
                AppUserId = "uid-1"
            },
            new () {
                Id = 2,
                Name = "Test 1",
                Description = "test description 1",
                PolicyType = 0,
                AppUserId = "uid-1"
            },
            new () {
                Id = 3,
                Name = "Test 2",
                Description = "test description 2",
                PolicyType = 0,
                AppUserId = "uid-1"
            }
        };

        return collection;
    }
}
