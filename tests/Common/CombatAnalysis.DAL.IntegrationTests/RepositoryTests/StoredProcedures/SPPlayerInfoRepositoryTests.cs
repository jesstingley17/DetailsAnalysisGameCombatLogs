using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.IntegrationTests.Data;
using CombatAnalysis.DAL.Repositories;

namespace CombatAnalysis.DAL.IntegrationTests.RepositoryTests.StoredProcedures;

[CollectionDefinition("SQL Server Tests")]
public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture> { }

[Collection("SQL Server Tests")]
public class SPPlayerInfoRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByCombatPlayerIdAsync_Collection_ShouldReturnDamageDoneByCombatPlayerId()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedDamageDoneTestDataAsync(context);

        const int combatPlayerId = 5;
        var repo = new PlayerInfoPaginationRepository<DamageDone>(context);

        // Act
        var result = await repo.GetByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_Collection_ShouldReturnDamageDoneByCombatPlayerIdUsePagination()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedDamageDoneTestDataAsync(context);

        const int combatPlayerId = 5;
        const int page = 1;
        const int pageSize = 10;
        var repo = new PlayerInfoPaginationRepository<DamageDone>(context);

        // Act
        var result = await repo.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_Collection_ShouldReturnEmptyCollectionByCombatPlayerId()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedDamageDoneTestDataAsync(context);

        const int combatPlayerId = 1;
        var repo = new PlayerInfoPaginationRepository<DamageDone>(context);

        // Act
        var result = await repo.GetByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_Collection_ShouldReturnEmptyCollectionByCombatPlayerIdUsePagination()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedDamageDoneTestDataAsync(context);

        const int combatPlayerId = 1;
        const int page = 1;
        const int pageSize = 10;
        var repo = new PlayerInfoPaginationRepository<DamageDone>(context);

        // Act
        var result = await repo.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await transaction.RollbackAsync();
    }
}
