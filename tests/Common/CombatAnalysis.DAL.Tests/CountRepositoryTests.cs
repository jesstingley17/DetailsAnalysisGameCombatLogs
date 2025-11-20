using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Repositories;
using CombatAnalysis.UserDAL.Tests.Factory;

namespace CombatAnalysis.DAL.Tests;

public class CountRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CountByCombatPlayerIdAsync_Count_ShouldReturnCountEntityByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;

        using var context = CreateInMemoryContext(nameof(CountByCombatPlayerIdAsync_Count_ShouldReturnCountEntityByCombatPlayerId));

        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

        var repo = new CountRepository<DamageDone>(context);

        // Act
        var result = await repo.CountByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task CountByCombatPlayerIdAsync_Count_ShouldReturnZeroEntityByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 2;

        using var context = CreateInMemoryContext(nameof(CountByCombatPlayerIdAsync_Count_ShouldReturnZeroEntityByCombatPlayerId));

        await context.Set<DamageDone>().AddRangeAsync(TestDataFactory.CreateDamageDonColelction());
        await context.SaveChangesAsync();

        var repo = new CountRepository<DamageDone>(context);

        // Act
        var result = await repo.CountByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.Equal(0, result);
    }
}
