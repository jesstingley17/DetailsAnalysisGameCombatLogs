using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserDAL.IntegrationTests.Factory;

internal static class FriendTestDataFactory
{
    public static Friend Create(
        int? id = null,
        string? whoFriendId = null,
        string? forWhomId = null
        )
    {
        var entity = new Friend(
            Id: id ?? 1,
            WhoFriendId: whoFriendId ?? "uid-1-1",
            ForWhomId: forWhomId ?? "uid-2-1"
        );

        return entity;
    }

    public static Friend[] CreateCollection(
        int size = 3 
        )
    {
        var collection = new Friend[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new Friend(
                Id: 1 + i,
                WhoFriendId: $"uid-1-{i}",
                ForWhomId: $"uid-2-{i}"
            );
        }

        return collection;
    }
}
