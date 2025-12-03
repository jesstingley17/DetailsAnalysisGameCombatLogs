using CombatAnalysis.CommunicationDAL.Repositories.StoredProcedures;
using CombatAnalysis.DAL.IntegrationTests.Data;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.RepositoryTests.StoredProcedures;

[Collection("SQL Server Tests")]
public class SPUserPostRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByCommunityIdAsync_Collection_ShouldReturnCollectionOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userId = "uid-1";
        const int pageSize = 5;

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.GetByAppUserIdAsync(userId, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetMoreByCommunityIdAsync_Collection_ShouldReturnMoreCollectionOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userId = "uid-1";
        const int pageSize = 1;
        const int offset = 1;

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.GetMoreByAppUserIdAsync(userId, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Single(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetNewByCommunityIdAsync_Collection_ShouldReturnNewestCollectionOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userId = "uid-1";
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/25/2025");

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.GetNewByAppUserIdAsync(userId, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByListOfCommunityIdAsync_Collection_ShouldReturnCollectionOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userIds = "uid-1,uid-2";
        const int pageSize = 5;

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.GetByListOfAppUserIdAsync(userIds, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetMoreByListOfCommunityIdAsync_Collection_ShouldReturnMoreCollectionOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userIds = "uid-1,uid-2";
        const int pageSize = 5;
        const int offset = 1;

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.GetMoreByListOfAppUserIdAsync(userIds, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetNewByListOfCommunityIdAsync_Collection_ShouldReturnMoreCollectionOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userIds = "uid-1,uid-2";
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/25/2025");

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.GetNewByListOfAppUserIdAsync(userIds, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CountByCommunityIdAsync_Count_ShouldReturnCountOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        const string userIds = "uid-1";

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.CountByAppUserIdAsync(userIds);

        // Assert
        Assert.Equal(2, result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CountByListOfCommunityIdAsync_Count_ShouldReturnCountOfUserPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedUserPostTestDataAsync(context);

        string[] userIds = ["uid-1", "uid-2"];

        var repo = new SPUserPostRepository(context);

        // Act
        var result = await repo.CountByListOfAppUserIdAsync(userIds);

        // Assert
        Assert.Equal(3, result);

        await transaction.RollbackAsync();
    }
}
