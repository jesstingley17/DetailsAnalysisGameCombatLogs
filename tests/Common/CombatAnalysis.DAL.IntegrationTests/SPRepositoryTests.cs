using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.IntegrationTests.Data;
using CombatAnalysis.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.IntegrationTests;

[Collection("SQL Server Tests")]
public class SPRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task CreateAsync_DamageDone_ShouldReturnCreatedDamageDone()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var damageDone = new DamageDone { Spell = "Test spell", Value = 50, Time = TimeSpan.FromSeconds(40), Creator = "Player-1", Target = "Enemy-1", DamageType = 0, IsPeriodicDamage = false, IsPet = false, CombatPlayerId = 1 };
        var repo = new GenericRepository<DamageDone>(context);

        // Act
        var createdDamageDone = await repo.CreateAsync(damageDone);

        // Assert
        var existDamageDone = await context.Set<DamageDone>().FirstOrDefaultAsync(d => d.Spell == "Test spell");
        Assert.NotNull(createdDamageDone);
        Assert.NotNull(existDamageDone);
        Assert.Equal(damageDone.Spell, existDamageDone.Spell);

        await transaction.RollbackAsync();
    }

    [Fact]
    public async Task CreateAsync_DamageDone_ShouldNotBeCompromisedBySQLInjection()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        var damageDone = new DamageDone { Spell = "DROP TABLE DamageDone;", Value = 50, Time = TimeSpan.FromSeconds(40), Creator = "Player-1", Target = "Enemy-1", DamageType = 0, IsPeriodicDamage = false, IsPet = false, CombatPlayerId = 1 };
        var repo = new GenericRepository<DamageDone>(context);

        // Act
        await repo.CreateAsync(damageDone);

        // Assert
        var createdEntity = await context.Set<DamageDone>().FirstOrDefaultAsync(d => d.Value == 50);
        Assert.NotNull(createdEntity);

        await transaction.RollbackAsync();
    }
}
