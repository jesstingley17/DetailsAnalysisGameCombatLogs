using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.IntegrationTests.Data;
using CombatAnalysis.DAL.Repositories;

namespace CombatAnalysis.DAL.IntegrationTests.RepositoryTests;

[Collection("SQL Server Tests")]
public class GenericRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedTestDataAsync(context);

        var repo = new GenericRepository<DamageDone>(context);
        var damageDone = new DamageDone()
        {
            Creator = "Solinx",
            Target = "Boss",
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = 1,
        };

        // Act
        var result = await repo.CreateAsync(damageDone);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, context.Set<DamageDone>().Count());

        await context.Database.RollbackTransactionAsync();

        await SqlServerFixture.Drop(context);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedTestDataAsync(context);

        const int id = 1;
        const string spell = "Damage ability";
        var repo = new GenericRepository<DamageDone>(context);
        var updatedDamageDone = new DamageDone()
        {
            Id = id,
            Creator = "Solinx",
            Target = "Boss",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = 1,
        };

        // Act
        await repo.UpdateAsync(updatedDamageDone);
        var updatedEntity = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal(id, updatedEntity.Id);
        Assert.Equal(spell, updatedEntity.Spell);

        await context.Database.RollbackTransactionAsync();

        await SqlServerFixture.Drop(context);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedTestDataAsync(context);

        const int id = 2;
        var repo = new GenericRepository<DamageDone>(context);

        // Act
        var col = await repo.GetAllAsync();
        await repo.DeleteAsync(id);

        // Assert
        Assert.Equal(1, context.Set<DamageDone>().Count());

        await context.Database.RollbackTransactionAsync();

        await SqlServerFixture.Drop(context);
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllElements()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedTestDataAsync(context);

        var repo = new GenericRepository<DamageDone>(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        await context.Database.RollbackTransactionAsync();

        await SqlServerFixture.Drop(context);
    }

    [Fact]
    public async Task GetByIdAsync_Entity_ShouldReturnEntityById()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedTestDataAsync(context);

        const int id = 1;
        var repo = new GenericRepository<DamageDone>(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);

        await context.Database.RollbackTransactionAsync();

        await SqlServerFixture.Drop(context);
    }

    [Fact]
    public async Task GetByParamAsync_Collection_ShouldReturnElementsByParam()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        await SqlServerFixture.SeedTestDataAsync(context);

        const string spell = "Test spell";
        var repo = new GenericRepository<DamageDone>(context);

        // Act
        var result = await repo.GetByParamAsync(nameof(DamageDone.Spell), spell);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        await context.Database.RollbackTransactionAsync();

        await SqlServerFixture.Drop(context);
    }
}
