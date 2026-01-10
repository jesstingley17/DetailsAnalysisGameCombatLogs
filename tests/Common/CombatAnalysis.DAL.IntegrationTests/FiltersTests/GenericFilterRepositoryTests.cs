using CombatAnalysis.DAL.Repositories.Filters;
using CombatAnalysis.DAL.IntegrationTests.RepositoryTests;
using CombatAnalysis.DAL.Entities.CombatPlayerData;

namespace CombatAnalysis.DAL.IntegrationTests.FiltersTests;

public class GenericFilterRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetTargetNamesByCombatPlayerIdAsync_Collection_ShouldReturnTargetNamesByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;

        using var context = CreateInMemoryContext(nameof(GetTargetNamesByCombatPlayerIdAsync_Collection_ShouldReturnTargetNamesByCombatPlayerId));

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

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None);

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
        const string target = "Boss";

        using var context = CreateInMemoryContext(nameof(CountTargetByCombatPlayerIdAsync_Count_ShouldReturnCountTargetsByCombatPlayerIdAndTargetName));
        context.Set<DamageDone>().AddRange(new DamageDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        },
        new DamageDone
        {
            Id = 2,
            Creator = "Solinx",
            Target = target,
            Spell = "Check",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:12"),
            Value = 100,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        });

        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.CountTargetByCombatPlayerIdAsync(combatPlayerId, target, CancellationToken.None);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetByTargetAsync_Collection_ShouldReturnDamageDonesByTargetName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Boss";

        using var context = CreateInMemoryContext(nameof(GetByTargetAsync_Collection_ShouldReturnDamageDonesByTargetName));

        context.Set<DamageDone>().Add(new DamageDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetByTargetAsync(combatPlayerId, target, 1, 10, CancellationToken.None);

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
        const string target = "Boss";
        const int value = 250;

        using var context = CreateInMemoryContext(nameof(GetTargetValueByCombatPlayerIdAsync_Value_ShouldReturnValueBySelectedTarget));
        context.Set<DamageDone>().AddRange(new DamageDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        },
        new DamageDone
        {
            Id = 2,
            Creator = "Solinx",
            Target = target,
            Spell = "Check",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 50,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target, CancellationToken.None);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public async Task GetCreatorNamesByCombatPlayerIdAsync_Collection_ShouldReturnCreatorNamesByByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Boss";

        using var context = CreateInMemoryContext(nameof(GetCreatorNamesByCombatPlayerIdAsync_Collection_ShouldReturnCreatorNamesByByCombatPlayerId));
        context.Set<DamageDone>().Add(new DamageDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None);

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
        const string creator = "Solinx";

        using var context = CreateInMemoryContext(nameof(CountCreatorByCombatPlayerIdAsync_Count_ShouldReturnCountCreatorsByCombatPlayerIdAndTargetName));
        context.Set<DamageDone>().Add(new DamageDone
        {
            Id = 1,
            Creator = creator,
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

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetByCreatorAsync_Collection_ShouldReturnDamageDonesByCreatorName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string creator = "Solinx";

        using var context = CreateInMemoryContext(nameof(GetByCreatorAsync_Collection_ShouldReturnDamageDonesByCreatorName));

        context.Set<DamageDone>().Add(new DamageDone
        {
            Id = 1,
            Creator = creator,
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

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetByCreatorAsync(combatPlayerId, creator, 1, 10, CancellationToken.None);

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

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None);

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
        context.Set<DamageDone>().AddRange(new DamageDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Boss",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        },
        new DamageDone
        {
            Id = 2,
            Creator = "Solinx",
            Target = "Boss",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:14"),
            Value = 112,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell, CancellationToken.None);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetBySpellAsync_Collection_ShouldReturnDamageDonesBySpellName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string spell = "Check";

        using var context = CreateInMemoryContext(nameof(GetBySpellAsync_Collection_ShouldReturnDamageDonesBySpellName));

        context.Set<DamageDone>().Add(new DamageDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Boss",
            Spell = spell,
            IsPeriodicDamage = false,
            Time = TimeSpan.Parse("00:01:10"),
            Value = 200,
            DamageType = 0,
            IsPet = false,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<DamageDone>(context);

        // Act
        var result = await repo.GetBySpellAsync(combatPlayerId, spell, 1, 10, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
}
