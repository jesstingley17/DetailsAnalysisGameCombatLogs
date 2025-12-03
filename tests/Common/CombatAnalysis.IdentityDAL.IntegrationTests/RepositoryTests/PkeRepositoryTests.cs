using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.IntegrationTests.Factory;
using CombatAnalysis.IdentityDAL.Repositories;

namespace CombatAnalysis.IdentityDAL.IntegrationTests.RepositoryTests;

public class PkeRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        const string clientId = "uid-1";
        const string authorizationCode = "auth code 1";
        const string codeChallenge = "code ch 1";
        const string codeChallengeMethod = "code ch method 1";
        const string redirectUri = "redirect 1";
        const int expiryTimeMins = 5;

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldCreateEntity));

        var repo = new PkeRepository(context);

        // Act
        await repo.CreateAsync(clientId, authorizationCode, codeChallenge, codeChallengeMethod, redirectUri, expiryTimeMins);

        // Assert
        Assert.NotNull(context.Set<AuthorizationCodeChallenge>().Find(authorizationCode));
        Assert.NotEmpty(context.Set<AuthorizationCodeChallenge>());
        Assert.Single(context.Set<AuthorizationCodeChallenge>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnExistedEntityById()
    {
        // Arrange
        const string id = "uid-1-1";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_ShouldReturnExistedEntityById));
        await context.Set<AuthorizationCodeChallenge>().AddRangeAsync(AuthorizationCodeChallengeTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new PkeRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task MarkCodeAsUsedAsync_Entity_ShouldUpdateEntity()
    {
        // Arrange
        const string id = "uid-1-1";

        using var context = CreateInMemoryContext(nameof(MarkCodeAsUsedAsync_Entity_ShouldUpdateEntity));
        await context.Set<AuthorizationCodeChallenge>().AddRangeAsync(AuthorizationCodeChallengeTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new PkeRepository(context);

        var authrizationCode = AuthorizationCodeChallengeTestDataFactory.Create(authorizationCode: id, isUsed: true);

        // Act
        await repo.MarkCodeAsUsedAsync(authrizationCode);

        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.True(result.IsUsed);
    }

    [Fact]
    public async Task RemoveExpiredCodesAsync_ShouldRemoveExpiredCodes()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(RemoveExpiredCodesAsync_ShouldRemoveExpiredCodes));
        await context.Set<AuthorizationCodeChallenge>().AddRangeAsync(AuthorizationCodeChallengeTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new PkeRepository(context);

        // Act
        await repo.RemoveExpiredCodesAsync();

        // Assert
        Assert.NotEmpty(context.Set<AuthorizationCodeChallenge>());
        Assert.Equal(3, context.Set<AuthorizationCodeChallenge>().Count());
    }
}
