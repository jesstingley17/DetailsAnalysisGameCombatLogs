using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.IntegrationTests.Factory;
using CombatAnalysis.UserDAL.Repositories;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace CombatAnalysis.UserDAL.IntegrationTests.RepositoryTests;

public class UserRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldAddEntityAndReturnCreatedEntity()
    {
        // Arrange
        const string username = "Alice12";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldAddEntityAndReturnCreatedEntity));

        var repo = new UserRepository(mockMultiplexer.Object, context);

        var entity = AppUserTestDataFactory.Create(username: username);

        // Act
        var result = await repo.CreateAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.Single(context.Set<AppUser>());
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllEntities()
    {
        // Arrange
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllEntities));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(),
            AppUserTestDataFactory.Create()
        );
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

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
        const string id = "uid-1";
        const string entityUsername = "Charlie45";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnCorrectEntity));
        await context.Set<AppUser>().AddAsync(AppUserTestDataFactory.Create(id: id, username: entityUsername));
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityUsername, result!.Username);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-22";
        const string updatedUsername = "Alice99";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        var user = AppUserTestDataFactory.Create(id: id, username: updatedUsername);

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateExistedEntityById));
        await context.Set<AppUser>().AddAsync(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var updateEntity = user with { Username = updatedUsername };
        await repo.UpdateAsync(id, updateEntity);

        var updatedEntity = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal(id, updatedEntity.Id);
        Assert.Equal(updatedUsername, updatedEntity.Username);
    }

    [Fact]
    public async Task UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-22";
        const string updatedUsername = "Alice99";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        var user = AppUserTestDataFactory.Create(id: "uid-3", username: updatedUsername);

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById));
        await context.Set<AppUser>().AddAsync(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var updateEntity = user with { Id = id, Username = updatedUsername };

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(id, updateEntity));
    }

    [Fact]
    public async Task UpdateAsync_ThrowInvalidOperationException_ShouldNotUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-22";
        const string updatedUsername = "Alice99";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        var user = AppUserTestDataFactory.Create(id: id, username: updatedUsername);

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ThrowInvalidOperationException_ShouldNotUpdateExistedEntityById));
        await context.Set<AppUser>().AddAsync(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var updateEntity = user with { Id = "uid-3", Username = updatedUsername };

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateAsync(id, updateEntity));
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const string id = "uid-22";
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(DeleteAsync_True_ShouldDeleteEntity));
        await context.Set<AppUser>().AddAsync(AppUserTestDataFactory.Create(id: id));
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var entityDeleted = await repo.DeleteAsync(id);

        // Assert
        Assert.True(entityDeleted);
        Assert.Empty(context.Set<AppUser>());
    }

    [Fact]
    public async Task DeleteAsync_False_ShouldNotDeleteEntity()
    {
        // Arrange
        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(DeleteAsync_False_ShouldNotDeleteEntity));
        await context.Set<AppUser>().AddAsync(AppUserTestDataFactory.Create());
        await context.SaveChangesAsync();

        var repo = new UserRepository(mockMultiplexer.Object, context);

        // Act
        var entityDeleted = await repo.DeleteAsync("uid-1");

        // Assert
        Assert.False(entityDeleted);
        Assert.NotEmpty(context.Set<AppUser>());
    }

    [Fact]
    public async Task FindByIdentityUserIdAsync_Entity_ShouldReturnFilteredResults()
    {
        // Arrange
        const string identityUserId = "uid-12";

        var mockMultiplexer = new Mock<IConnectionMultiplexer>();

        using var context = CreateInMemoryContext(nameof(FindByIdentityUserIdAsync_Entity_ShouldReturnFilteredResults));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(identityUserId: identityUserId),
            AppUserTestDataFactory.Create()
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
    public async Task FindByUsernameStartAtAsync_Collection_ShouldReturnFilteredResultsFromRedis()
    {
        // Arrange
        const string startAt = "Al";
        const string username1 = "Alice";
        const string username2 = "Alex";
        const string username3 = "Kiril";

        using var context = CreateInMemoryContext(nameof(FindByUsernameStartAtAsync_Collection_ShouldReturnFilteredResultsFromRedis));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(username: username1),
            AppUserTestDataFactory.Create(username: username2),
            AppUserTestDataFactory.Create(username: username3)
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
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Username == "Alice");
        Assert.Contains(result, r => r.Username == "Alex");
    }

    [Fact]
    public async Task FindByUsernameStartAtAsync_Collection_ShouldReturnFilteredResultsFromDB()
    {
        // Arrange
        const string startAt = "Kir";
        const string username1 = "Alice";
        const string username2 = "Alex";
        const string username3 = "Kiril";

        using var context = CreateInMemoryContext(nameof(FindByUsernameStartAtAsync_Collection_ShouldReturnFilteredResultsFromDB));
        await context.Set<AppUser>().AddRangeAsync(
            AppUserTestDataFactory.Create(username: username1),
            AppUserTestDataFactory.Create(username: username2),
            AppUserTestDataFactory.Create(username: username3)
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
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Contains(result, r => r.Username == username3);
    }
}
