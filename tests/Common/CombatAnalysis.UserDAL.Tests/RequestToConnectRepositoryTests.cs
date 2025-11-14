using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;

namespace CombatAnalysis.UserDAL.Tests;

public class RequestToConnectRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Arrange
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        var now = DateTimeOffset.Now;

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldAddEntity));
        var repo = new GenericRepository<RequestToConnect, int>(context);
        var user = new RequestToConnect(
            Id: 1,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        );

        // Act
        var result = await repo.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.ToAppUserId);
        Assert.Equal(user2Id, result.AppUserId);
        Assert.Equal(now, result.When);
        Assert.Single(context.Set<RequestToConnect>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_ShouldReturnAllEntities));
        context.Set<RequestToConnect>().AddRange(
            new RequestToConnect(
                Id: 1,
                ToAppUserId: "uid-222",
                AppUserId: "uid-223",
                When: DateTimeOffset.Now.AddSeconds(5)
            ),
            new RequestToConnect(
                Id: 2,
                ToAppUserId: "uid-224",
                AppUserId: "uid-223",
                When: DateTimeOffset.Now.AddSeconds(7)
            )
        );

        await context.SaveChangesAsync();

        var repo = new GenericRepository<RequestToConnect, int>(context);

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
        const int requestToConnectId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        var now = DateTimeOffset.Now;

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_ShouldReturnCorrectEntity));

        context.Set<RequestToConnect>().Add(new RequestToConnect(
            Id: requestToConnectId,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        ));
        await context.SaveChangesAsync();

        var repo = new GenericRepository<RequestToConnect, int>(context);

        // Act
        var result = await repo.GetByIdAsync(requestToConnectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.ToAppUserId);
        Assert.Equal(user2Id, result.AppUserId);
        Assert.Equal(now, result.When);
        Assert.Single(context.Set<RequestToConnect>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        const int requestToConnectId = 1;
        const string user1Id = "uid-222";
        const string user2Id = "uid-223";
        var now = DateTimeOffset.Now;

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldRemoveEntity));
        context.Set<RequestToConnect>().Add(new RequestToConnect(
            Id: requestToConnectId,
            ToAppUserId: user1Id,
            AppUserId: user2Id,
            When: now
        ));
        await context.SaveChangesAsync();

        var repo = new GenericRepository<RequestToConnect, int>(context);

        // Act
        var rowsAffected = await repo.DeleteAsync(requestToConnectId);

        // Assert
        Assert.Equal(1, rowsAffected);
        Assert.Empty(context.Set<RequestToConnect>());
    }

    [Fact]
    public async Task GetByParamAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredToAppUserId = "uid-223";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_ShouldReturnFilteredResults));
        context.Set<RequestToConnect>().AddRange(
            new RequestToConnect(
                Id: 1,
                ToAppUserId: "uid-223",
                AppUserId: "uid-223",
                When: DateTimeOffset.Now.AddSeconds(5)
            ),
            new RequestToConnect(
                Id: 2,
                ToAppUserId: "uid-224",
                AppUserId: "uid-223",
                When: DateTimeOffset.Now.AddSeconds(7)
            )
        );
        await context.SaveChangesAsync();

        var repo = new GenericRepository<RequestToConnect, int>(context);

        // Act
        var result = await repo.GetByParamAsync(C => C.ToAppUserId, filteredToAppUserId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(filteredToAppUserId, result.First().ToAppUserId);
    }
}
