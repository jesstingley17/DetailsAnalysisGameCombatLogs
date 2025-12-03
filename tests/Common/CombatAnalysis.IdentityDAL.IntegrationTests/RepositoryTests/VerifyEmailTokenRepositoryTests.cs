using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.IntegrationTests.Factory;
using CombatAnalysis.IdentityDAL.Repositories;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.RepositoryTests;

public class VerifyEmailTokenRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const int id = 1;

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldCreateEntity));

        var repo = new VerifyEmailTokenRepository(context);

        var token = VerifyEmailTokenTestDataFactory.Create(id: id);

        // Act
        await repo.CreateAsync(token);

        // Assert
        Assert.NotNull(context.Set<VerifyEmailToken>().Find(id));
        Assert.NotEmpty(context.Set<VerifyEmailToken>());
        Assert.Single(context.Set<VerifyEmailToken>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistedEntityById()
    {
        // Arrange
        const int id = 1;
        const string email = "email2";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateExistedEntityById));
        await context.Set<VerifyEmailToken>().AddRangeAsync(VerifyEmailTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new VerifyEmailTokenRepository(context);

        var token = VerifyEmailTokenTestDataFactory.Create(id: id, email: email);

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
        await context.Set<VerifyEmailToken>().AddRangeAsync(VerifyEmailTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new VerifyEmailTokenRepository(context);

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

        await context.Set<VerifyEmailToken>().AddRangeAsync(VerifyEmailTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new VerifyEmailTokenRepository(context);

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
        await context.Set<VerifyEmailToken>().AddRangeAsync(VerifyEmailTokenTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new VerifyEmailTokenRepository(context);

        // Act
        await repo.RemoveExpiredVerifyEmailTokenAsync();

        // Assert
        Assert.NotEmpty(context.Set<VerifyEmailToken>());
        Assert.Equal(3, context.Set<VerifyEmailToken>().Count());
    }
}
