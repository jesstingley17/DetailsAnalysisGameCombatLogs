using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.IntegrationTests.Factory;
using CombatAnalysis.IdentityDAL.Repositories;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.RepositoryTests;

public class ResetTokenRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const int id = 1;

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldCreateEntity));

        var repo = new ResetTokenRepository(context);

        var token = ResetTokenTestDataFactory.Create(id: id);

        // Act
        await repo.CreateAsync(token);

        // Assert
        Assert.NotNull(context.Set<ResetToken>().Find(id));
        Assert.NotEmpty(context.Set<ResetToken>());
        Assert.Single(context.Set<ResetToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistedEntityById()
    {
        // Arrange
        const int id = 1;
        const string email = "email2";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateExistedEntityById));
        await context.Set<ResetToken>().AddRangeAsync(ResetTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new ResetTokenRepository(context);

        var token = ResetTokenTestDataFactory.Create(id: id, email: email);

        // Act
        await repo.UpdateAsync(id, token);

        var updatedEntity = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal(id, updatedEntity.Id);
        Assert.Equal(email, updatedEntity.Email);
    }

    [Fact]
    public async Task GetByIdAsync_Entity_ShouldReturnEntity()
    {
        // Arrange
        const int id = 1;

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnEntity));
        await context.Set<ResetToken>().AddRangeAsync(ResetTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new ResetTokenRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByTokenAsync_Entity_ShouldReturnEntityByToken()
    {
        // Arrange
        const string token = "token 23-0";

        using var context = CreateInMemoryContext(nameof(GetByTokenAsync_Entity_ShouldReturnEntityByToken));

        await context.Set<ResetToken>().AddRangeAsync(ResetTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new ResetTokenRepository(context);

        // Act
        var result = await repo.GetByTokenAsync(token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
    }

    [Fact]
    public async Task RemoveExpiredTokensAsync_ShouldRemoveExpiredCodes()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(RemoveExpiredTokensAsync_ShouldRemoveExpiredCodes));
        await context.Set<ResetToken>().AddRangeAsync(ResetTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new ResetTokenRepository(context);

        // Act
        await repo.RemoveExpiredResetTokenAsync();

        // Assert
        Assert.NotEmpty(context.Set<ResetToken>());
        Assert.Equal(3, context.Set<ResetToken>().Count());
    }
}
