using CombatAnalysis.UserBL.Services;
using CombatAnalysis.UserDAL.Interfaces;
using Moq;

namespace CombatAnalysis.UserBL.Tests.ServicesTests;

public class UserTransactionServiceTests
{
    [Fact]
    public async Task BeginTransactionAsync_ShouldBeginTransaction()
    {
        // Arrange
        var mockContext = new Mock<IContextService>();

        var service = new UserTransactionService(mockContext.Object);

        // Act
        await service.BeginTransactionAsync();

        // Verify correct method calls
        mockContext.Verify(r => r.BeginAsync(), Times.Once);
    }

    [Fact]
    public async Task BeginTransactionAsync_ShouldCommitTransaction()
    {
        // Arrange
        var mockContext = new Mock<IContextService>();

        var service = new UserTransactionService(mockContext.Object);

        // Act
        await service.CommitTransactionAsync();

        // Verify correct method calls
        mockContext.Verify(r => r.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RollbackTransactionAsync_ShouldRollbackTransaction()
    {
        // Arrange
        var mockContext = new Mock<IContextService>();

        var service = new UserTransactionService(mockContext.Object);

        // Act
        await service.RollbackTransactionAsync();

        // Verify correct method calls
        mockContext.Verify(r => r.RollbackAsync(), Times.Once);
    }
}
