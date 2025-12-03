using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;
using CombatAnalysis.UserDAL.IntegrationTests.Factory;

namespace CombatAnalysis.UserDAL.IntegrationTests.RepositoryTests;

public class GenericRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        const string id = "uid-1";

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateEntityAndReturnCreatedEntity));
        var repo = new GenericRepository<Customer, string>(context);

        var entity = CustomerTestDataFactory.Create(id: id);

        // Act
        var result = await repo.CreateAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.NotEmpty(context.Set<Customer>());
        Assert.Single(context.Set<Customer>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-1";
        const string newCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateExistedEntityById));
        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        var entity = CustomerTestDataFactory.Create(id: id, city: newCity);

        // Act
        await repo.UpdateAsync(id, entity);

        var updatedEntity = await repo.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal(id, updatedEntity.Id);
        Assert.Equal(newCity, updatedEntity.City);
    }

    [Fact]
    public async Task UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById()
    {
        // Arrange
        const string id = "uid-12";
        const string newCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById));
        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        var entity = CustomerTestDataFactory.Create(id: id, city: newCity);

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(id, entity));
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldRemoveEntity()
    {
        // Arrange
        const string id = "uid-1";

        using var context = CreateInMemoryContext(nameof(DeleteAsync_True_ShouldRemoveEntity));
        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var entityDeleted = await repo.DeleteAsync(id);

        // Assert
        Assert.True(entityDeleted);
        Assert.NotEmpty(context.Set<Customer>());
        Assert.Equal(2, context.Set<Customer>().Count());
    }

    [Fact]
    public async Task DeleteAsync_False_ShouldNotRemoveEntityAsEntityDoesNotExist()
    {
        // Arrange
        const string id = "uid-12";

        using var context = CreateInMemoryContext(nameof(DeleteAsync_False_ShouldNotRemoveEntityAsEntityDoesNotExist));
        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var entityDeleted = await repo.DeleteAsync(id);

        // Assert
        Assert.False(entityDeleted);
        Assert.NotEmpty(context.Set<Customer>());
        Assert.Equal(3, context.Set<Customer>().Count());
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllEntities()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllEntities));
        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Entity_ShouldReturnCorrectEntity()
    {
        // Arrange
        const string customerId = "uid-1";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Entity_ShouldReturnCorrectEntity));

        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var result = await repo.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
    }

    [Fact]
    public async Task GetByParamAsync_Collection_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredCity = "city-1";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_Collection_ShouldReturnFilteredResults));
        await context.Set<Customer>().AddRangeAsync(CustomerTestDataFactory.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var result = await repo.GetByParamAsync(C => C.City, filteredCity);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(filteredCity, result.First().City);
    }
}
