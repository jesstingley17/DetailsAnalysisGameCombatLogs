using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.IntegrationTests.Data;
using CombatAnalysis.DAL.Repositories.StoredProcedures;
using CombatAnalysis.UserDAL.IntegrationTests.Factory;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.IntegrationTests.RepositoryTests.StoredProcedures;

[Collection("SQL Server Tests")]
public class SPGenericRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task CreateAsync_DamageDone_ShouldReturnCreatedDamageDone()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        const string spell = "Test spell";

        var damageDone = DamageDoneTestDataFactory.Create(spell: spell);
        var repo = new SPGenericRepository<DamageDone>(context);

        // Act
        var createdDamageDone = await repo.CreateAsync(damageDone);

        // Assert
        var existDamageDone = await context.Set<DamageDone>().FirstOrDefaultAsync(d => d.Spell == spell);
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
        const string spell = "DROP TABLE DamageDone;";
        const int value = 50;

        var damageDone = DamageDoneTestDataFactory.Create(spell: spell, value: value);
        var repo = new SPGenericRepository<DamageDone>(context);

        // Act
        await repo.CreateAsync(damageDone);

        // Assert
        var createdEntity = await context.Set<DamageDone>().FirstOrDefaultAsync(d => d.Value == value);
        Assert.NotNull(createdEntity);

        await transaction.RollbackAsync();
    }
}
