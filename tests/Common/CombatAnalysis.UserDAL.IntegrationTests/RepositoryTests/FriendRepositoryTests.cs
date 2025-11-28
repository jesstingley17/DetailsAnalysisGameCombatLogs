using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;
using CombatAnalysis.UserDAL.IntegrationTests.Factory;

namespace CombatAnalysis.UserDAL.IntegrationTests.RepositoryTests;

public class FriendRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldAddEntityAndReturnCreated()
    {
        // Arrange
        const string user1Username = "Alice12";
        const string user1Id = "uid-222";
        const string user2Username = "Drivet5";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldAddEntityAndReturnCreated));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(id: user1Id, username: user1Username),
            AppUserTestDataFactory.Create(id: user2Id, username: user2Username)
        );
        await context.SaveChangesAsync();

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
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 2;

        using var context = CreateInMemoryContext(nameof(DeleteAsync_True_ShouldDeleteEntity));
        await context.Set<Friend>().AddAsync(new Friend(
            Id: id,
            WhoFriendId: "uid-222",
            ForWhomId: "uid-223"
        ));
        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var entityDeleted = await repo.DeleteAsync(id);

        // Assert
        Assert.True(entityDeleted);
        Assert.Empty(context.Set<Friend>());
    }

    [Fact]
    public async Task DeleteAsync_False_ShouldNotDeleteEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(DeleteAsync_False_ShouldNotDeleteEntity));
        await context.Set<Friend>().AddAsync(new Friend(
            Id: 2,
            WhoFriendId: "uid-222",
            ForWhomId: "uid-223"
        ));
        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var entityDeleted = await repo.DeleteAsync(2222);

        // Assert
        Assert.False(entityDeleted);
        Assert.NotEmpty(context.Set<Friend>());
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllEntities()
    {
        // Arrange
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        const string user3Id = "uid-224";

        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllEntities));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(id: user1Id),
            AppUserTestDataFactory.Create(id: user2Id),
            AppUserTestDataFactory.Create(id: user3Id)
        );
        await context.Set<Friend>().AddRangeAsync(
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
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Entity_ShouldReturnCorrectEntity()
    {
        // Arrange
        const int id = 2;
        const string user1Username = "Alice12";
        const string user1Id = "uid-222";
        const string user2Username = "Drivet5";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnCorrectEntity));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(id: user1Id, username: user1Username),
            AppUserTestDataFactory.Create(id: user2Id, username: user2Username)
        );
        await context.Set<Friend>().AddAsync(new Friend(
            Id: id,
            WhoFriendId: user1Id,
            ForWhomId: user2Id
        ));
        await context.SaveChangesAsync();

        var repo = new FriendRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

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
    public async Task GetByParamAsync_Colelction_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredWhoFriendId = "uid-222";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_Colelction_ShouldReturnFilteredResults));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(id: "uid-222"),
            AppUserTestDataFactory.Create(id: "uid-223"),
            AppUserTestDataFactory.Create(id: "uid-224")
        );
        await context.Set<Friend>().AddRangeAsync(
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
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(filteredWhoFriendId, result.First().WhoFriendId);
    }
}
