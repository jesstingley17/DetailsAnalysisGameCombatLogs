using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;
using CombatAnalysis.UserDAL.Tests.Factory;

namespace CombatAnalysis.UserDAL.Tests;

public class FriendRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Arrange
        const string user1Username = "Alice12";
        const string user1Id = "uid-222";
        const string user2Username = "Drivet5";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldAddEntity));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(id: user1Id, username: user1Username),
            TestDataFactory.CreateAppUser(id: user2Id, username: user2Username)
        );

        var repo = new FriendRepository(context);
        var friend = new Friend(
            Id: 1,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        );

        // Act
        var result = await repo.CreateAsync(friend);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.WhoFriendId);
        Assert.Equal(user1Username, result.WhoFriendUsername);
        Assert.Equal(user2Id, result.ForWhomId);
        Assert.Equal(user2Username, result.ForWhomUsername);
        Assert.NotEqual(result.WhoFriendId, result.ForWhomId);
        Assert.Single(context.Set<Friend>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        const string user3Id = "uid-224";

        using var context = CreateInMemoryContext(nameof(GetAllAsync_ShouldReturnAllEntities));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(id: user1Id),
            TestDataFactory.CreateAppUser(id: user2Id),
            TestDataFactory.CreateAppUser(id: user3Id)
        );

        context.Set<Friend>().AddRange(
            new Friend(
                Id: 1,
                WhoFriendId: "uid-222",
                ForWhomId: "uid-223"
            ),
            new Friend(
                Id: 2,
                WhoFriendId: "uid-222",
                ForWhomId: "uid-224"
            )
        );

        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectEntity()
    {
        // Arrange
        const string user1Username = "Alice12";
        const string user1Id = "uid-222";
        const string user2Username = "Drivet5";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_ShouldReturnCorrectEntity));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(id: user1Id, username: user1Username),
            TestDataFactory.CreateAppUser(id: user2Id, username: user2Username)
        );

        var friend = new Friend(
            Id: 2,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        );
        context.Set<Friend>().Add(friend);
        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var result = await repo.GetByIdAsync(friend.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.WhoFriendId);
        Assert.Equal(user1Username, result.WhoFriendUsername);
        Assert.Equal(user2Id, result.ForWhomId);
        Assert.Equal(user2Username, result.ForWhomUsername);
        Assert.NotEqual(result.WhoFriendId, result.ForWhomId);
        Assert.Single(context.Set<Friend>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldRemoveEntity));
        var friend = new Friend(
            Id: 2,
            WhoFriendId: "uid-222",
            ForWhomId: "uid-223"
        );
        context.Set<Friend>().Add(friend);
        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var rowsAffected = await repo.DeleteAsync(friend.Id);

        // Assert
        Assert.Equal(1, rowsAffected);
        Assert.Empty(context.Set<Friend>());
    }

    [Fact]
    public async Task GetByParamAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredWhoFriendId = "uid-222";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_ShouldReturnFilteredResults));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(id: "uid-222"),
            TestDataFactory.CreateAppUser(id: "uid-223"),
            TestDataFactory.CreateAppUser(id: "uid-224")
        );

        context.Set<Friend>().AddRange(
            new Friend(
                Id: 1,
                WhoFriendId: "uid-222",
                ForWhomId: "uid-223"
            ),
            new Friend(
                Id: 2,
                WhoFriendId: "uid-224",
                ForWhomId: "uid-223"
            )
        );
        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var result = await repo.GetByParamAsync(nameof(Friend.WhoFriendId), filteredWhoFriendId);

        // Assert
        Assert.Single(result);
        Assert.Equal(filteredWhoFriendId, result.First().WhoFriendId);
    }
}
