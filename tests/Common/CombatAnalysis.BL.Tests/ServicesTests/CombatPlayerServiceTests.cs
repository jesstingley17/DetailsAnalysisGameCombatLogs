using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatPlayerServiceTests
{
    [Fact]
    public async Task CreateBatchAsync_ShouldCreateCollectionOfEntity()
    {
        // Arrange
        var entityDtoCollection = CombatPlayerTestDataFactory.CreateDtoCollection();
        var entityCollection = CombatPlayerTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICombatPlayerRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayer>>(entityDtoCollection)).Returns(entityCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<CombatPlayer>>(It.IsAny<IEnumerable<CombatPlayerDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<CombatPlayer>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByCombatIdAsync_Collection_ShouldReturnCollection()
    {
        // Arrange
        const int combatPlayerId = 1;

        var entityCollection = CombatPlayerTestDataFactory.CreateCollection();
        var entityDtoCollection = CombatPlayerTestDataFactory.CreateDtoCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICombatPlayerRepository>();

        mockRepository.Setup(m => m.GetByCombatIdAsync(combatPlayerId, CancellationToken.None)).ReturnsAsync(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayerDto>>(entityCollection)).Returns(entityDtoCollection);

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        var result = await service.GetByCombatIdAsync(combatPlayerId, CancellationToken.None);

        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCombatIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(r => r.Map<IEnumerable<CombatPlayerDto>>(It.IsAny<IEnumerable<CombatPlayer>>()), Times.Once);
    }

    [Fact]
    public async Task GetByCombatIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAnyEntity()
    {
        // Arrange
        const int combatPlayerId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICombatPlayerRepository>();

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByCombatIdAsync(combatPlayerId, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCombatIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
