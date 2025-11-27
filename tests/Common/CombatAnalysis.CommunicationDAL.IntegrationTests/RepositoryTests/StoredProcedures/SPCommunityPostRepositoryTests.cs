using CombatAnalysis.CommunicationDAL.Repositories.StoredProcedures;
using CombatAnalysis.DAL.IntegrationTests.Data;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.RepositoryTests.StoredProcedures;

[CollectionDefinition("SQL Server Tests")]
public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture> { }

[Collection("SQL Server Tests")]
public class SPCommunityPostRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByCommunityIdAsync_Collection_ShouldReturnCollectionOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const int communityId = 1;
        const int pageSize = 5;

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.GetByCommunityIdAsync(communityId, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetMoreByCommunityIdAsync_Collection_ShouldReturnMoreCollectionOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const int communityId = 1;
        const int pageSize = 1;
        const int offset = 1;

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.GetMoreByCommunityIdAsync(communityId, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Single(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetNewByCommunityIdAsync_Collection_ShouldReturnNewestCollectionOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const int communityId = 1;
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/25/2025");

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.GetNewByCommunityIdAsync(communityId, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByListOfCommunityIdAsync_Collection_ShouldReturnCollectionOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const string communityIds = "1,2";
        const int pageSize = 5;

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.GetByListOfCommunityIdAsync(communityIds, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetMoreByListOfCommunityIdAsync_Collection_ShouldReturnMoreCollectionOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const string communityIds = "1,2";
        const int pageSize = 5;
        const int offset = 1;

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.GetMoreByListOfCommunityIdAsync(communityIds, offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetNewByListOfCommunityIdAsync_Collection_ShouldReturnMoreCollectionOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const string communityIds = "1,2";
        DateTimeOffset checkFrom = DateTimeOffset.Parse("11/25/2025");

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.GetNewByListOfCommunityIdAsync(communityIds, checkFrom);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CountByCommunityIdAsync_Count_ShouldReturnCountOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        const int communityIds = 1;

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.CountByCommunityIdAsync(communityIds);

        // Assert
        Assert.Equal(2, result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CountByListOfCommunityIdAsync_Count_ShouldReturnCountOfCommunityPosts()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedCommunityPostTestDataAsync(context);

        int[] communityIds = [ 1, 2 ];

        var repo = new SPCommunityPostRepository(context);

        // Act
        var result = await repo.CountByListOfCommunityIdAsync(communityIds);

        // Assert
        Assert.Equal(3, result);

        await transaction.RollbackAsync();
    }
}
