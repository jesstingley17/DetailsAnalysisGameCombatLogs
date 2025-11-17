using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Repositories.SQL.Filters;

namespace CombatAnalysis.DAL.Tests;

public class DamageFilterRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetDamageByEachTargetAsync_Collection_ShouldReturnCombatTargetsByCombatId()
    {
        // Arrange
        const int combatId = 1;
        const int combatLogId = 1;
        const int combatPlayerId = 1;

        using var context = CreateInMemoryContext(nameof(GetDamageByEachTargetAsync_Collection_ShouldReturnCombatTargetsByCombatId));

        context.Set<Combat>().Add(new Combat
        {
            Id = combatId,
            LocallyNumber = 1,
            DungeonName = "Dung",
            Name = "Test",
            Difficulty = 1,
            DamageDone = 3456,
            HealDone = 200,
            DamageTaken = 0,
            EnergyRecovery = 0,
            IsWin = true,
            StartDate = DateTime.Now,
            FinishDate = DateTime.Now.AddSeconds(70),
            IsReady = true,
            CombatLogId = combatLogId
        });
        context.Set<CombatPlayer>().Add(new CombatPlayer
        {
            Id = 1,
            Username = "Solinx",
            PlayerId = "uid-22",
            AverageItemLevel = 345,
            ResourcesRecovery = 0,
            DamageDone = 345,
            HealDone = 0,
            DamageTaken = 0,
            CombatId = combatId,
        });
        context.Set<DamageDone>().Add(new DamageDone
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
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new DamageFilterRepository(context);

        // Act
        var result = await repo.GetDamageByEachTargetAsync(combatId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.NotEmpty(result.First());
        Assert.Single(result.First());
    }
}
