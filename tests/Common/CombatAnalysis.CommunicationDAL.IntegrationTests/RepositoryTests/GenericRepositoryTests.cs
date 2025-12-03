using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.IntegrationTests.Factory;
using CombatAnalysis.CommunicationDAL.Repositories;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationDAL.IntegrationTests.RepositoryTests;

public class GenericRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateNewEntityAndReturnCreatedEntity));

        var repo = new GenericRepository<CommunityUser, string>(context);
        var entity = CommunityUserTestDataFactory.Create();

        // Act
        var result = await repo.CreateAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Single(context.Set<CommunityUser>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEcistedEntityById()
    {
        // Arrange
        const string id = "uid-1";
        const string username = "Kiril";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateEcistedEntityById));
        var repo = new GenericRepository<CommunityUser, string>(context);
        await context.Set<CommunityUser>().AddRangeAsync(CommunityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var entity = CommunityUserTestDataFactory.Create(username: username);

        // Act
        await repo.UpdateAsync(id, entity);

        var updatedEntity = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal(id, updatedEntity.Id);
        Assert.Equal(username, updatedEntity.Username);
    }

    [Fact]
    public async Task UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-1345";
        const string username = "Kiril";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateEcistedEntityById));
        var repo = new GenericRepository<CommunityUser, string>(context);
        await context.Set<CommunityUser>().AddRangeAsync(CommunityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var entity = CommunityUserTestDataFactory.Create(id: id, username: username);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(id, entity));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        const string id = "uid-1";

        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldDeleteEntity));

        var repo = new GenericRepository<CommunityUser, string>(context);
        await context.Set<CommunityUser>().AddRangeAsync(CommunityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(id);

        // Assert
        Assert.Equal(2, context.Set<CommunityUser>().Count());
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllElements()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllElements));

        var repo = new GenericRepository<CommunityUser, string>(context);
        await context.Set<CommunityUser>().AddRangeAsync(CommunityUserTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Entity_ShouldReturnEntityById()
    {
        // Arrange
        const string id = "uid-1";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnEntityById));

        var repo = new GenericRepository<CommunityUser, string>(context);
        await context.Set<CommunityUser>().AddRangeAsync(CommunityUserTestDataFactory.Create());
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByParamAsync_Collection_ShouldReturnElementsByParam()
    {
        // Arrange
        const string username = "Solinx";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_Collection_ShouldReturnElementsByParam));

        var repo = new GenericRepository<CommunityUser, string>(context);
        await context.Set<CommunityUser>().AddRangeAsync(CommunityUserTestDataFactory.Create());
        await context.SaveChangesAsync();

        Expression<Func<CommunityUser, string>> expression = n => n.Username;

        // Act
        var result = await repo.GetByParamAsync(expression, username);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
    }
}
