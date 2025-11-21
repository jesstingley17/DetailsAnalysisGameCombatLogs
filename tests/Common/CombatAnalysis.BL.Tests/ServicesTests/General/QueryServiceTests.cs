using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests.General;

public class QueryServiceTests
{
    [Fact]
    public async Task GetAllAsync_CollectionOfCombats_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        var combats = CombatTestDataFactory.CreateCollection();
        var combatsDto = CombatTestDataFactory.CreateDtoColelction();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatDto>>(combats)).Returns(combatsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(combats);

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        var combats = new List<Combat>();
        var combatsDto = new List<CombatDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatDto>>(combats)).Returns(combatsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(combats);

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Combat_ShouldReturnCombatById()
    {
        // Arrange
        const int combatId = 1;

        var combatDto = CombatTestDataFactory.CreateDto();
        var combat = CombatTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<CombatDto>(combat)).Returns(combatDto);

        mockRepository.Setup(m => m.GetByIdAsync(combatId)).ReturnsAsync(combat);

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(combatId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(combatId, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const int combatId = 1;

        var combatDto = CombatTestDataFactory.CreateDto();
        var combat = CombatTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<CombatDto>(combat)).Returns(combatDto);

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(combatId);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnEntity()
    {
        // Arrange
        const int combatId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByIdAsync(combatId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetByParamAsync_CollectionOfCombats_ShouldReturnFewElementsInCollection()
    {
        // Arrange
        const int combatLogId = 1;

        var combats = CombatTestDataFactory.CreateCollection();
        var combatsDto = CombatTestDataFactory.CreateDtoColelction();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatDto>>(combats)).Returns(combatsDto);

        mockRepository.Setup(m => m.GetByParamAsync(nameof(Combat.CombatLogId), combatLogId)).ReturnsAsync(combats);

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByParamAsync(nameof(Combat.CombatLogId), combatLogId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task GetByParamAsync_EmptyCollection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int combatLogId = 2;

        var combats = new List<Combat>();
        var combatsDto = new List<CombatDto>();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        var service = new QueryService<CombatDto, Combat>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByParamAsync(nameof(Combat.CombatLogId), combatLogId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByParamAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }
}
