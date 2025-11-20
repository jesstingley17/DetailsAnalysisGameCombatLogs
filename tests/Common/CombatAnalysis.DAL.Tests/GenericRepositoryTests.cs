using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Repositories;
using CombatAnalysis.UserDAL.Tests.Factory;

namespace CombatAnalysis.DAL.Tests;

public class GenericRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity));

        var repo = new GenericRepository<DamageDone>(context);
        var damageDone = new DamageDone()
        {
            Id = 1,
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
        Assert.Single(context.Set<DamageDone>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const string spell = "Damage ability";
        const int id = 1;

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateEntity));

        var repo = new GenericRepository<DamageDone>(context);
        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

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
        Assert.Equal(spell, updatedEntity.Spell);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldDeleteEntity));

        var repo = new GenericRepository<DamageDone>(context);
        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(id);

        // Assert
        Assert.Equal(2, context.Set<DamageDone>().Count());
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllElements()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllElements));

        var repo = new GenericRepository<DamageDone>(context);
        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Entity_ShouldReturnEntityById()
    {
        // Arrange
        const int id = 1;

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnEntityById));

        var repo = new GenericRepository<DamageDone>(context);
        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByParamAsync_Collection_ShouldReturnElementsByParam()
    {
        // Arrange
        const string spell = "Test";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_Collection_ShouldReturnElementsByParam));

        var repo = new GenericRepository<DamageDone>(context);
        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByParamAsync(nameof(DamageDone.Spell), spell);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
}
