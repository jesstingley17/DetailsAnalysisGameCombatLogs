using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;

namespace CombatAnalysis.UserDAL.Tests.RepositoryTests;

public class GenericRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_Customer_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        const string user1Id = "uid-222";

        using var context = CreateInMemoryContext(nameof(CreateAsync_Customer_ShouldCreateEntityAndReturnCreatedEntity));
        var repo = new GenericRepository<Customer, string>(context);
        var customer = new Customer(
            Id: Guid.NewGuid().ToString(),
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: user1Id
        );

        // Act
        var result = await repo.CreateAsync(customer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.AppUserId);
        Assert.Single(context.Set<Customer>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEcistedEntityById()
    {
        // Arrange
        const string id = "uid-221";
        const string newCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ShouldUpdateEcistedEntityById));
        var repo = new GenericRepository<Customer, string>(context);
        await context.Set<Customer>().AddAsync(new Customer(
            Id: id,
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: "uid-222"
        ));
        await context.SaveChangesAsync();

        var customer = new Customer(
            Id: id,
            Country: "Belarus",
            City: newCity,
            PostalCode: 234234,
            AppUserId: "uid-222"
        );

        // Act
        await repo.UpdateAsync(id, customer);

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
        const string id = "uid-221";
        const string newCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ThrowKeyNotFoundException_ShouldNotUpdateExistedEntityById));
        var repo = new GenericRepository<Customer, string>(context);
        await context.Set<Customer>().AddAsync(new Customer(
            Id: "uid-220",
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: "uid-222"
        ));
        await context.SaveChangesAsync();

        var customer = new Customer(
            Id: id,
            Country: "Belarus",
            City: newCity,
            PostalCode: 234234,
            AppUserId: "uid-222"
        );

        // Act and Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(id, customer));
    }

    [Fact]
    public async Task UpdateAsync_ThrowInvalidOperationException_ShouldNotUpdateEntityById()
    {
        // Arrange
        const string id = "uid-221";
        const string newCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(UpdateAsync_ThrowInvalidOperationException_ShouldNotUpdateEntityById));
        var repo = new GenericRepository<Customer, string>(context);
        await context.Set<Customer>().AddAsync(new Customer(
            Id: id,
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: "uid-222"
        ));
        await context.SaveChangesAsync();

        var customer = new Customer(
            Id: "uid-222",
            Country: "Belarus",
            City: newCity,
            PostalCode: 234234,
            AppUserId: "uid-222"
        );

        // Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateAsync(id, customer));
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldRemoveEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(DeleteAsync_True_ShouldRemoveEntity));
        var customer = new Customer(
            Id: Guid.NewGuid().ToString(),
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: "uid-2"
        );
        await context.Set<Customer>().AddAsync(customer);
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var entityDeleted = await repo.DeleteAsync(customer.Id);

        // Assert
        Assert.True(entityDeleted);
        Assert.Empty(context.Set<Customer>());
    }

    [Fact]
    public async Task DeleteAsync_False_ShouldNotRemoveEntityAsEntityDoesNotExist()
    {
        // Arrange
        const string id = "uid-22";

        using var context = CreateInMemoryContext(nameof(DeleteAsync_False_ShouldNotRemoveEntityAsEntityDoesNotExist));
        var customer = new Customer(
            Id: id,
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: "uid-2"
        );
        await context.Set<Customer>().AddAsync(customer);
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var entityDeleted = await repo.DeleteAsync("uid-222");

        // Assert
        Assert.False(entityDeleted);
        Assert.NotEmpty(context.Set<Customer>());
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllEntities()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_Collection_ShouldReturnAllEntities));
        await context.Set<Customer>().AddRangeAsync(
            new Customer(
                Id: Guid.NewGuid().ToString(),
                Country: "Belarus",
                City: "Minsk",
                PostalCode: 234234,
                AppUserId: "uid-2"
            ),
            new Customer(
                Id: Guid.NewGuid().ToString(),
                Country: "Belarus",
                City: "Minsk",
                PostalCode: 234234,
                AppUserId: "uid-3"
            )
        );
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Customer_ShouldReturnCorrectEntity()
    {
        // Arrange
        var customerId = Guid.NewGuid().ToString();
        const string user1Id = "uid-222";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_Customer_ShouldReturnCorrectEntity));

        await context.Set<Customer>().AddAsync(new Customer(
            Id: customerId,
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: user1Id
        ));
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var result = await repo.GetByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.AppUserId);
        Assert.Single(context.Set<Customer>());
    }

    [Fact]
    public async Task GetByParamAsync_Collection_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_Collection_ShouldReturnFilteredResults));
        await context.Set<Customer>().AddRangeAsync(
            new Customer(
                Id: Guid.NewGuid().ToString(),
                Country: "Belarus",
                City: filteredCity,
                PostalCode: 234234,
                AppUserId: "uid-2"
            ),
            new Customer(
                Id: Guid.NewGuid().ToString(),
                Country: "Belarus",
                City: "Minsk",
                PostalCode: 234234,
                AppUserId: "uid-3"
            )
        );
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
