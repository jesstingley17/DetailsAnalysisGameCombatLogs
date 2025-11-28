using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.IntegrationTests.Factory;
using CombatAnalysis.IdentityDAL.Repositories;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.RepositoryTests;

public class IdentityUserRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task SaveAsync_ShouldSaveEntity()
    {
        // Arrange
        const string id = "uid-1";

        using var context = CreateInMemoryContext(nameof(SaveAsync_ShouldSaveEntity));
        var identityUser = IdentityUserTestDataFactory.Create(id: id);

        var repo = new IdentityUserRepository(context);

        // Act
        await repo.CreateAsync(identityUser);

        // Assert
        Assert.NotNull(context.Set<IdentityUser>().Find(id));
        Assert.NotEmpty(context.Set<IdentityUser>());
        Assert.Single(context.Set<IdentityUser>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-1-1";
        const string email = "email2";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateExistedEntityById));
        await context.Set<IdentityUser>().AddRangeAsync(IdentityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new IdentityUserRepository(context);

        var identityUser = IdentityUserTestDataFactory.Create(id: id, email: email);

        // Act
        await repo.UpdateAsync(id, identityUser);

        var updatedEntity = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal(id, updatedEntity.Id);
        Assert.Equal(email, updatedEntity.Email);
    }

    [Fact]
    public async Task GetAsync_Entity_ShouldReturnEntity()
    {
        // Arrange
        const string email = "email-0";

        using var context = CreateInMemoryContext(nameof(GetAsync_Entity_ShouldReturnEntity));
        await context.Set<IdentityUser>().AddRangeAsync(IdentityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new IdentityUserRepository(context);

        // Act
        var result = await repo.GetAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_Customer_ShouldReturnCorrectEntity()
    {
        // Arrange
        const string id = "uid-1-1";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Customer_ShouldReturnCorrectEntity));

        await context.Set<IdentityUser>().AddRangeAsync(IdentityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new IdentityUserRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task CheckByEmailAsync_True_ShouldReturnTrueAsEmailUsed()
    {
        // Arrange
        const string email = "email-1";

        using var context = CreateInMemoryContext(nameof(CheckByEmailAsync_True_ShouldReturnTrueAsEmailUsed));
        await context.Set<IdentityUser>().AddRangeAsync(IdentityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new IdentityUserRepository(context);

        // Act
        var result = await repo.CheckByEmailAsync(email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckByEmailAsync_False_ShouldReturnFalseAsEmailNotUsed()
    {
        // Arrange
        const string email = "email";

        using var context = CreateInMemoryContext(nameof(CheckByEmailAsync_False_ShouldReturnFalseAsEmailNotUsed));
        await context.Set<IdentityUser>().AddRangeAsync(IdentityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new IdentityUserRepository(context);

        // Act
        var result = await repo.CheckByEmailAsync(email);

        // Assert
        Assert.False(result);
    }
}
