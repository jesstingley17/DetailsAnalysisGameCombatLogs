using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.IntegrationTests.Factory;
using CombatAnalysis.CommunicationDAL.Repositories;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.RepositoryTests;

public class CommunityRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetAllWithPaginationAsync_Collection_ShouldReturnCollectionOfCommunity()
    {
        // Arrange
        const int pageSize = 1;

        using var context = CreateInMemoryContext(nameof(GetAllWithPaginationAsync_Collection_ShouldReturnCollectionOfCommunity));
        await context.Set<Community>().AddRangeAsync(CommunityTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new CommunityRepository(context);

        // Act
        var result = await repo.GetAllWithPaginationAsync(pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetMoreWithPaginationAsync_Collection_ShouldReturnMoreCollectionOfCommunityy()
    {
        // Arrange
        const int pageSize = 5;
        const int offset = 1;

        using var context = CreateInMemoryContext(nameof(GetMoreWithPaginationAsync_Collection_ShouldReturnMoreCollectionOfCommunityy));
        await context.Set<Community>().AddRangeAsync(CommunityTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new CommunityRepository(context);

        // Act
        var result = await repo.GetMoreWithPaginationAsync(offset, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CountAsync_Count_ShouldReturnCountOfCommunity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(CountAsync_Count_ShouldReturnCountOfCommunity));
        await context.Set<Community>().AddRangeAsync(CommunityTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new CommunityRepository(context);

        // Act
        var result = await repo.CountAsync();

        // Assert
        Assert.Equal(3, result);
    }
}
