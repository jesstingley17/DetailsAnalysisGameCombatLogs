using CombatAnalysis.BL.Services;
using CombatAnalysis.DAL.Interfaces;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatTransactionServiceTests
{
    [Fact]
    public async Task BeginTransactionAsync_ShouldBeginTransaction()
    {
        // Arrange
        var mockContext = new Mock<IContextService>();

        var service = new CombatTransactionService(mockContext.Object);

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

        var service = new CombatTransactionService(mockContext.Object);

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

        var service = new CombatTransactionService(mockContext.Object);

        // Act
        await service.RollbackTransactionAsync();

        // Verify correct method calls
        mockContext.Verify(r => r.RollbackAsync(), Times.Once);
    }
}
