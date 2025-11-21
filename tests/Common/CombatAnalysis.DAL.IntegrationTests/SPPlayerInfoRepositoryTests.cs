using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.IntegrationTests.Data;
using CombatAnalysis.DAL.Repositories;

namespace CombatAnalysis.DAL.IntegrationTests;

[CollectionDefinition("SQL Server Tests DamageDone")]
public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture> { }

[Collection("SQL Server Tests DamageDone")]
public class SPPlayerInfoRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetByCombatPlayerIdAsync_ShouldReturnDamageDoneByCombatPlayerId()
    {
        using var transaction = await _fixture.DbContext.Database.BeginTransactionAsync();

        // Arrange
        await _fixture.SeedTestDataAsync();

        const int combatPlayerId = 5;
        var repo = new PlayerInfoRepository<DamageDone>(_fixture.DbContext);

        // Act
        var damageDones = await repo.GetByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(damageDones);
        Assert.NotEmpty(damageDones);
        Assert.Equal(2, damageDones.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_ShouldReturnDamageDoneByCombatPlayerIdUsePagination()
    {
        using var transaction = await _fixture.DbContext.Database.BeginTransactionAsync();

        // Arrange
        await _fixture.SeedTestDataAsync();

        const int combatPlayerId = 5;
        const int page = 1;
        const int pageSize = 10;
        var repo = new PlayerInfoRepository<DamageDone>(_fixture.DbContext);

        // Act
        var damageDones = await repo.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        // Assert
        Assert.NotNull(damageDones);
        Assert.NotEmpty(damageDones);
        Assert.Equal(2, damageDones.Count());

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_ShouldReturnEmptyCollectionByCombatPlayerId()
    {
        using var transaction = await _fixture.DbContext.Database.BeginTransactionAsync();

        // Arrange
        await _fixture.SeedTestDataAsync();

        const int combatPlayerId = 1;
        var repo = new PlayerInfoRepository<DamageDone>(_fixture.DbContext);

        // Act
        var damageDones = await repo.GetByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(damageDones);
        Assert.Empty(damageDones);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_ShouldReturnEmptyCollectionByCombatPlayerIdUsePagination()
    {
        using var transaction = await _fixture.DbContext.Database.BeginTransactionAsync();

        // Arrange
        await _fixture.SeedTestDataAsync();

        const int combatPlayerId = 1;
        const int page = 1;
        const int pageSize = 10;
        var repo = new PlayerInfoRepository<DamageDone>(_fixture.DbContext);

        // Act
        var damageDones = await repo.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);

        // Assert
        Assert.NotNull(damageDones);
        Assert.Empty(damageDones);

        await transaction.RollbackAsync();
    }
}
