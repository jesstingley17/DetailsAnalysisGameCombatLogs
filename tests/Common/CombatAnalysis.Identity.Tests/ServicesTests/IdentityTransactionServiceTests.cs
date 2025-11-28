using AutoMapper;
using CombatAnalysis.Identity.Services;
using CombatAnalysis.Identity.Tests.Factory;
using CombatAnalysis.IdentityDAL.Interfaces;
using Moq;

namespace CombatAnalysis.Identity.Tests.ServicesTests;

public class IdentityTransactionServiceTests
{
    [Fact]
    public async Task BeginTransactionAsync_ShouldBeginTransaction()
    {
        // Arrange
        var entityDto = IdentityUserTestDataFactory.CreateDto();
        var entity = IdentityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IContextService>();

        mockRepository.Setup(m => m.BeginAsync()).Returns(Task.CompletedTask);

        var service = new IdentityTransactionService(mockRepository.Object);

        // Act
        await service.BeginTransactionAsync();

        // Assert and Verify correct method calls
        mockRepository.Verify(r => r.BeginAsync(), Times.Once);
    }

    [Fact]
    public async Task CommitTransactionAsync_ShouldDoCommitTransaction()
    {
        // Arrange
        var entityDto = IdentityUserTestDataFactory.CreateDto();
        var entity = IdentityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IContextService>();

        mockRepository.Setup(m => m.CommitAsync()).Returns(Task.CompletedTask);

        var service = new IdentityTransactionService(mockRepository.Object);

        // Act
        await service.CommitTransactionAsync();

        // Assert and Verify correct method calls
        mockRepository.Verify(r => r.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RollbackTransactionAsync_ShouldDoRollbackTransaction()
    {
        // Arrange
        var entityDto = IdentityUserTestDataFactory.CreateDto();
        var entity = IdentityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IContextService>();

        mockRepository.Setup(m => m.RollbackAsync()).Returns(Task.CompletedTask);

        var service = new IdentityTransactionService(mockRepository.Object);

        // Act
        await service.RollbackTransactionAsync();

        // Assert and Verify correct method calls
        mockRepository.Verify(r => r.RollbackAsync(), Times.Once);
    }
}
