using CombatAnalysis.DAL.IntegrationTests.Data;
using CombatAnalysis.DAL.Repositories.StoredProcedures;

namespace CombatAnalysis.DAL.IntegrationTests.RepositoryTests.StoredProcedures;

[Collection("SQL Server Tests")]
public class SPSpecScoreRepositoryTests(SqlServerFixture fixture)
{
    private readonly SqlServerFixture _fixture = fixture;

    [Fact]
    public async Task GetBySpecIdAsync_Collection_ShouldReturnSpecializationScoreCollection()
    {
        using var context = _fixture.CreateContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        // Arrange
        const int specId = 1;
        const int bossId = 1;

        await SqlServerFixture.SeedSpecializationScoreTestDataAsync(context);

        var repo = new SPSpecScoreRepository(context);

        // Act
        var result = await repo.GetBySpecIdAsync(specId, bossId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);

        await transaction.RollbackAsync();
    }
}
