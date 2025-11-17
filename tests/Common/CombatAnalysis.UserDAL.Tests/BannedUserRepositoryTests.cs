using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;

namespace CombatAnalysis.UserDAL.Tests;

public class BannedUserRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Arrange
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldAddEntity));
        var repo = new GenericRepository<BannedUser, int>(context);
        var user = new BannedUser(
            Id: 1,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        );

        // Act
        var result = await repo.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.WhomBannedId);
        Assert.Equal(user2Id, result.BannedUserId);
        Assert.Single(context.Set<BannedUser>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_ShouldReturnAllEntities));
        context.Set<BannedUser>().AddRange(
            new BannedUser(
                Id: 1,
                WhomBannedId: "uid-222",
                BannedUserId: "uid-223"
            ),
            new BannedUser(
                Id: 2,
                WhomBannedId: "uid-222",
                BannedUserId: "uid-224"
            )
        );

        await context.SaveChangesAsync();

        var repo = new GenericRepository<BannedUser, int>(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectEntity()
    {
        // Arrange
        const int bannedUserId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_ShouldReturnCorrectEntity));

        context.Set<BannedUser>().Add(new BannedUser(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        ));
        await context.SaveChangesAsync();

        var repo = new GenericRepository<BannedUser, int>(context);

        // Act
        var result = await repo.GetByIdAsync(bannedUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.WhomBannedId);
        Assert.Equal(user2Id, result.BannedUserId);
        Assert.Single(context.Set<BannedUser>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        const int bannedUserId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldRemoveEntity));
        context.Set<BannedUser>().Add(new BannedUser(
            Id: bannedUserId,
            WhomBannedId: user1Id,
            BannedUserId: user2Id
        ));
        await context.SaveChangesAsync();

        var repo = new GenericRepository<BannedUser, int>(context);

        // Act
        var rowsAffected = await repo.DeleteAsync(bannedUserId);

        // Assert
        Assert.Equal(1, rowsAffected);
        Assert.Empty(context.Set<BannedUser>());
    }

    [Fact]
    public async Task GetByParamAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredBannedUserId = "uid-223";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_ShouldReturnFilteredResults));
        context.Set<BannedUser>().AddRange(
            new BannedUser(
                Id: 1,
                WhomBannedId: "uid-222",
                BannedUserId: "uid-223"
            ),
            new BannedUser(
                Id: 2,
                WhomBannedId: "uid-222",
                BannedUserId: "uid-224"
            )
        );
        await context.SaveChangesAsync();

        var repo = new GenericRepository<BannedUser, int>(context);

        // Act
        var result = await repo.GetByParamAsync(b => b.BannedUserId, filteredBannedUserId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(filteredBannedUserId, result.First().BannedUserId);
    }
}
