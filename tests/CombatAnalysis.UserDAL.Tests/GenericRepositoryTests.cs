using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Repositories;

namespace CombatAnalysis.UserDAL.Tests;

public class GenericRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task CreateAsync_ShouldAddEntity()
    {
        // Arrange
        const string user1Id = "uid-222";

        using var context = CreateInMemoryContext(nameof(CreateAsync_ShouldAddEntity));
        var repo = new GenericRepository<Customer, string>(context);
        var user = new Customer(
            Id: Guid.NewGuid().ToString(),
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: user1Id
        );

        // Act
        var result = await repo.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.AppUserId);
        Assert.Single(context.Set<Customer>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(GetAllAsync_ShouldReturnAllEntities));
        context.Set<Customer>().AddRange(
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
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectEntity()
    {
        // Arrange
        const string user1Id = "uid-222";

        using var context = CreateInMemoryContext(nameof(GetByIdAsync_ShouldReturnCorrectEntity));

        var customer = new Customer(
            Id: Guid.NewGuid().ToString(),
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: user1Id
        );
        context.Set<Customer>().Add(customer);
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var result = await repo.GetByIdAsync(customer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user1Id, result.AppUserId);
        Assert.Single(context.Set<Customer>());
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        using var context = CreateInMemoryContext(nameof(DeleteAsync_ShouldRemoveEntity));
        var customer = new Customer(
            Id: Guid.NewGuid().ToString(),
            Country: "Belarus",
            City: "Minsk",
            PostalCode: 234234,
            AppUserId: "uid-2"
        );
        context.Set<Customer>().Add(customer);
        await context.SaveChangesAsync();

        var repo = new GenericRepository<Customer, string>(context);

        // Act
        var rowsAffected = await repo.DeleteAsync(customer.Id);

        // Assert
        Assert.Equal(1, rowsAffected);
        Assert.Empty(context.Set<Friend>());
    }

    [Fact]
    public async Task GetByParamAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        const string filteredCity = "Grodno";

        using var context = CreateInMemoryContext(nameof(GetByParamAsync_ShouldReturnFilteredResults));
        context.Set<Customer>().AddRange(
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
        Assert.Single(result);
        Assert.Equal(filteredCity, result.First().City);
    }
}
