using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Repositories.SQL.Filters;

namespace CombatAnalysis.DAL.Tests.FilterRepositories;

public class GenericFilterRepositoryHealDoneTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetTargetNamesByCombatPlayerIdAsync_Collection_ShouldReturnTargetNamesByCombatPlayerId()
    {
        // Arrange
        const int combatPlayerId = 1;

        using var context = CreateInMemoryContext(nameof(GetTargetNamesByCombatPlayerIdAsync_Collection_ShouldReturnTargetNamesByCombatPlayerId));

        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Kiril",
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

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
        const string target = "Kiril";

        using var context = CreateInMemoryContext(nameof(CountTargetByCombatPlayerIdAsync_Count_ShouldReturnCountTargetsByCombatPlayerIdAndTargetName));
        context.Set<HealDone>().AddRange(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        },
        new HealDone
        {
            Id = 2,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:12"),
            Overheal = 0,
            Value = 100,
            CombatPlayerId = combatPlayerId,
        });

        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

        // Act
        var result = await repo.CountTargetByCombatPlayerIdAsync(combatPlayerId, target);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetByTargetAsync_Collection_ShouldReturnHealDonesByTargetName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Kiril";

        using var context = CreateInMemoryContext(nameof(GetByTargetAsync_Collection_ShouldReturnHealDonesByTargetName));

        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

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
        const string target = "Kiril";
        const int value = 300;

        using var context = CreateInMemoryContext(nameof(GetTargetValueByCombatPlayerIdAsync_Value_ShouldReturnValueBySelectedTarget));
        context.Set<HealDone>().AddRange(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        },
        new HealDone
        {
            Id = 2,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:12"),
            Overheal = 0,
            Value = 100,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

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
        const string target = "Kiril";

        using var context = CreateInMemoryContext(nameof(GetCreatorNamesByCombatPlayerIdAsync_Collection_ShouldReturnCreatorNamesByByCombatPlayerId));
        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = target,
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

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
        const string creator = "Solinx";

        using var context = CreateInMemoryContext(nameof(CountCreatorByCombatPlayerIdAsync_Count_ShouldReturnCountCreatorsByCombatPlayerIdAndTargetName));
        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = creator,
            Target = "Kiril",
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

        // Act
        var result = await repo.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetByCreatorAsync_Collection_ShouldReturnHealDonesByCreatorName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string creator = "Solinx";

        using var context = CreateInMemoryContext(nameof(GetByCreatorAsync_Collection_ShouldReturnHealDonesByCreatorName));

        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = creator,
            Target = "Kiril",
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

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

        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Kiril",
            Spell = "Test",
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

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
        context.Set<HealDone>().AddRange(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Kiril",
            Spell = spell,
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        },
        new HealDone
        {
            Id = 2,
            Creator = "Solinx",
            Target = "Kiril",
            Spell = spell,
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:12"),
            Overheal = 0,
            Value = 100,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

        // Act
        var result = await repo.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetBySpellAsync_Collection_ShouldReturnHealDonesBySpellName()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string spell = "Check";

        using var context = CreateInMemoryContext(nameof(GetBySpellAsync_Collection_ShouldReturnHealDonesBySpellName));

        context.Set<HealDone>().Add(new HealDone
        {
            Id = 1,
            Creator = "Solinx",
            Target = "Kiril",
            Spell = spell,
            IsCrit = false,
            IsAbsorbed = false,
            Time = TimeSpan.Parse("00:01:10"),
            Overheal = 10,
            Value = 200,
            CombatPlayerId = combatPlayerId,
        });
        await context.SaveChangesAsync();

        var repo = new GeneralFilterRepositroy<HealDone>(context);

        // Act
        var result = await repo.GetBySpellAsync(combatPlayerId, spell, 1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
}
