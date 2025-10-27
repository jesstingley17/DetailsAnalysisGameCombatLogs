using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;
using CombatAnalysis.UserDAL.Tests.Factory;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace CombatAnalysis.UserDAL.Tests;

public class UserRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Arrange
        const string username = "Alice12";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldAddEntity));
        var repo = new UserRepository(mockMultiplexer.Object, context);
        var user = TestDataFactory.CreateAppUser(username: username);

        // Act
        var result = await repo.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Single(context.Set<AppUser>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(GetAllAsync_ShouldReturnAllEntities));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(),
            TestDataFactory.CreateAppUser()
        );

        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectEntity()
    {
        // Arrange
        const string entityUsername = "Charlie45";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_ShouldReturnCorrectEntity));
        var user = TestDataFactory.CreateAppUser(username: entityUsername);
        context.Set<AppUser>().Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var result = await repo.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityUsername, result!.Username);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingEntity()
    {
        // Arrange
        const string updatedUsername = "Alice99";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateExistingEntity));
        var user = TestDataFactory.CreateAppUser(username: updatedUsername);
        context.Set<AppUser>().Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var updated = user with { Username = updatedUsername };
        var rowsAffected = await repo.UpdateAsync(updated.Id, updated);

        // Assert
        Assert.Equal(1, rowsAffected);
        Assert.Equal(updatedUsername, context.Set<AppUser>().First().Username);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldRemoveEntity));
        var user = TestDataFactory.CreateAppUser();
        context.Set<AppUser>().Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var rowsAffected = await repo.DeleteAsync(user.Id);

        // Assert
        Assert.Equal(1, rowsAffected);
        Assert.Empty(context.Set<AppUser>());
    }

    [Fact]
    public async Task FindByIdentityUserIdAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        const string identityUserId = "uid-12";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(FindByIdentityUserIdAsync_ShouldReturnFilteredResults));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(identityUserId: identityUserId),
            TestDataFactory.CreateAppUser()
        );
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var result = await repo.FindByIdentityUserIdAsync(identityUserId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(identityUserId, result.IdentityUserId);
    }

    [Fact]
    public async Task FindByUsernameStartAtAsync_ShouldReturnFilteredResultsFromRedis()
    {
        // Arrange
        const string startAt = "Al";
        const string username1 = "Alice";
        const string username2 = "Alex";
        const string username3 = "Kiril";

        using var context = CreateInMemoryContext(nameof(FindByUsernameStartAtAsync_ShouldReturnFilteredResultsFromRedis));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(username: username1),
            TestDataFactory.CreateAppUser(username: username2),
            TestDataFactory.CreateAppUser(username: username3)
        );
        await context.SaveChangesAsync();

        var mockDb = new Mock<IDatabase>();

        var users = new[]
        {
            new AppUser("1", "Alice", "A", "Smith", 111, DateTimeOffset.Now, "test", 1, "id1"),
            new AppUser("2", "Alex", "A", "Brown", 222, DateTimeOffset.Now, "test", 1, "id2")
        };

        // Fake Redis sorted set result
        var redisArray = users
            .Select(u => RedisResult.Create((RedisValue)u.Username.ToLower()))
            .ToArray();

        var redisArrayResult = RedisResult.Create(redisArray);

        string min = $"[{startAt.ToLower()}";
        string max = $"[{startAt.ToLower()}\uffff";

        mockDb
            .Setup(m => m.ExecuteAsync("ZRANGE", "usernames", min, max, "BYLEX"))
            .Returns(Task.FromResult(redisArrayResult));

        foreach (var u in users)
        {
            var json = JsonSerializer.Serialize(u);
            mockDb.Setup(m => m.StringGetAsync($"user:{u.Username.ToLower()}", It.IsAny<CommandFlags>()))
                  .ReturnsAsync(json);
        }

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();
        mockMultiplexer
                .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDb.Object);

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var result = await repo.FindByUsernameStartAtAsync(startAt);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Username == "Alice");
        Assert.Contains(result, r => r.Username == "Alex");
    }

    [Fact]
    public async Task FindByUsernameStartAtAsync_ShouldReturnFilteredResultsFromDB()
    {
        // Arrange
        const string startAt = "Kir";
        const string username1 = "Alice";
        const string username2 = "Alex";
        const string username3 = "Kiril";

        using var context = CreateInMemoryContext(nameof(FindByUsernameStartAtAsync_ShouldReturnFilteredResultsFromDB));
        context.Set<AppUser>().AddRange(
            TestDataFactory.CreateAppUser(username: username1),
            TestDataFactory.CreateAppUser(username: username2),
            TestDataFactory.CreateAppUser(username: username3)
        );
        await context.SaveChangesAsync();

        var mockDb = new Mock<IDatabase>();

        var users = new[]
{
            new AppUser("1", "Alice", "A", "Smith", 111, DateTimeOffset.Now, "test", 1, "id1"),
            new AppUser("2", "Alex", "A", "Brown", 222, DateTimeOffset.Now, "test", 1, "id2")
        };

        // Fake Redis sorted set result
        var redisArray = users
            .Select(u => RedisResult.Create((RedisValue)u.Username.ToLower()))
            .ToArray();

        string min = $"[{startAt.ToLower()}";
        string max = $"[{startAt.ToLower()}\uffff";

        mockDb
            .Setup(m => m.ExecuteAsync("ZRANGE", "usernames", min, max, "BYLEX"))
            .ReturnsAsync(RedisResult.Create(redisArray));

        mockDb
            .Setup(m => m.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(RedisValue.Null);

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();
        mockMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
               .Returns(mockDb.Object);

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var result = await repo.FindByUsernameStartAtAsync(startAt);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, r => r.Username == username3);
    }
}
