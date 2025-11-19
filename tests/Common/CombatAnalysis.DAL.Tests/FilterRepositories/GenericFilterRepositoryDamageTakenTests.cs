using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Repositories.SQL.Filters;

namespace CombatAnalysis.DAL.Tests.FilterRepositories;

public class GenericFilterRepositoryDamageTakenTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetTargetNamesByCombatPlayerIdAsync_Collection_ShouldReturnTargetNamesByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;

        using var context = CreateInMemoryContext(nameof(GetTargetNamesByCombatPlayerIdAsync_Collection_ShouldReturnTargetNamesByCombatPlayerId));

        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Boss",
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task CountTargetByCombatPlayerIdAsync_Count_ShouldReturnCountTargetsByCombatPlayerIdAndTargetName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Solinx";

        using var context = CreateInMemoryContext(nameof(CountTargetByCombatPlayerIdAsync_Count_ShouldReturnCountTargetsByCombatPlayerIdAndTargetName));
        context.Set<DamageTaken>().AddRange(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        },
        new DamageTaken
        {
            Id = 2,
            Creator = "Boss",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:11"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });

        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.CountTargetByCombatPlayerIdAsync(combatPlayerId, target);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetByTargetAsync_Collection_ShouldReturnDamageTakensByTargetName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Solinx";

        using var context = CreateInMemoryContext(nameof(GetByTargetAsync_Collection_ShouldReturnDamageTakensByTargetName));

        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetByTargetAsync(combatPlayerId, target, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetTargetValueByCombatPlayerIdAsync_Value_ShouldReturnValueBySelectedTarget()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Solinx";
        const int value = 250;

        using var context = CreateInMemoryContext(nameof(GetTargetValueByCombatPlayerIdAsync_Value_ShouldReturnValueBySelectedTarget));
        context.Set<DamageTaken>().AddRange(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        },
        new DamageTaken
        {
            Id = 2,
            Creator = "Boss",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:11"),
            Value = 50,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public async Task GetCreatorNamesByCombatPlayerIdAsync_Collection_ShouldReturnCreatorNamesByByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Solinx";

        using var context = CreateInMemoryContext(nameof(GetCreatorNamesByCombatPlayerIdAsync_Collection_ShouldReturnCreatorNamesByByCombatPlayerId));
        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task CountCreatorByCombatPlayerIdAsync_Count_ShouldReturnCountCreatorsByCombatPlayerIdAndTargetName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string creator = "Boss";

        using var context = CreateInMemoryContext(nameof(CountCreatorByCombatPlayerIdAsync_Count_ShouldReturnCountCreatorsByCombatPlayerIdAndTargetName));
        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = creator,
            Target = "Solinx",
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetByCreatorAsync_Collection_ShouldReturnDamageTakensByCreatorName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string creator = "Boss";

        using var context = CreateInMemoryContext(nameof(GetByCreatorAsync_Collection_ShouldReturnDamageTakensByCreatorName));

        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = creator,
            Target = "Solinx",
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetByCreatorAsync(combatPlayerId, creator, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetSpellNamesByCombatPlayerIdAsync_Collection_ShouldReturnSpellNamesByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;

        using var context = CreateInMemoryContext(nameof(GetSpellNamesByCombatPlayerIdAsync_Collection_ShouldReturnSpellNamesByCombatPlayerId));

        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = "Solinx",
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task CountSpellByCombatPlayerIdAsync_Count_ShouldReturnCountSpellsByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string spell = "Test";

        using var context = CreateInMemoryContext(nameof(CountSpellByCombatPlayerIdAsync_Count_ShouldReturnCountSpellsByCombatPlayerId));
        context.Set<DamageTaken>().AddRange(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = "Solinx",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        },
        new DamageTaken
        {
            Id = 2,
            Creator = "Boss",
            Target = "Solinx",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:11"),
            Value = 50,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetBySpellAsync_Collection_ShouldReturnDamageTakensBySpellName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string spell = "Check";

        using var context = CreateInMemoryContext(nameof(GetBySpellAsync_Collection_ShouldReturnDamageTakensBySpellName));

        context.Set<DamageTaken>().Add(new DamageTaken
        {
            Id = 1,
            Creator = "Boss",
            Target = "Solinx",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            ActualValue = 200,
            DamageTakenType = 0,
            Resisted = 0,
            Absorbed = 0,
            Blocked = 0,
            RealDamage = 200,
            Mitigated = 0,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageTaken>(context);

        // Act
        var result = await repo.GetBySpellAsync(combatPlayerId, spell, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
}
