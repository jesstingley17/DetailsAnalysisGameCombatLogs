using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests.General;

public class PlayerInfoServiceTests
{
    [Fact]
    public async Task GetByCombatPlayerIdAsync_CollectionOfCombats_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;

        var damages = DamageDoneTestDataFactory.CreateCollection();
        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();

        var mockMapper = new Mock<IMapper>();
        var mockPaginationRepository = new Mock<IPlayerInfoPaginationRepository<DamageDone>>();
        var mockRepository = new Mock<IPlayerInfoRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None)).ReturnsAsync(damages);

        var service = new PlayerInfoPaginationService<DamageDoneDto, DamageDone>(mockPaginationRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;

        var damages = DamageDoneTestDataFactory.CreateCollection();
        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();

        var mockMapper = new Mock<IMapper>();
        var mockPaginationRepository = new Mock<IPlayerInfoPaginationRepository<DamageDone>>();
        var mockRepository = new Mock<IPlayerInfoRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockPaginationRepository.Setup(m => m.GetByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None)).ReturnsAsync(damages);

        var service = new PlayerInfoPaginationService<DamageDoneDto, DamageDone>(mockPaginationRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None));

        // Verify correct method calls
        mockPaginationRepository.Verify(r => r.GetByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_CollectionOfCombats_ShouldReturnAFewElementsInCollectionUsePagination()
    {
        // Arrange
        const int combatPlayerId = 1;
        const int page = 1;
        const int pageSize = 5;

        var damages = DamageDoneTestDataFactory.CreateCollection();
        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();

        var mockMapper = new Mock<IMapper>();
        var mockPaginationRepository = new Mock<IPlayerInfoPaginationRepository<DamageDone>>();
        var mockRepository = new Mock<IPlayerInfoRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockPaginationRepository.Setup(m => m.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, CancellationToken.None)).ReturnsAsync(damages);

        var service = new PlayerInfoPaginationService<DamageDoneDto, DamageDone>(mockPaginationRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockPaginationRepository.Verify(r => r.GetByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollectionUsePagination()
    {
        // Arrange
        const int combatPlayerId = 0;
        const int page = 1;
        const int pageSize = 5;

        var damages = DamageDoneTestDataFactory.CreateCollection();
        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();

        var mockMapper = new Mock<IMapper>();
        var mockPaginationRepository = new Mock<IPlayerInfoPaginationRepository<DamageDone>>();
        var mockRepository = new Mock<IPlayerInfoRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockPaginationRepository.Setup(m => m.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, CancellationToken.None)).ReturnsAsync(damages);

        var service = new PlayerInfoPaginationService<DamageDoneDto, DamageDone>(mockPaginationRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize, CancellationToken.None));

        // Verify correct method calls
        mockPaginationRepository.Verify(r => r.GetByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
