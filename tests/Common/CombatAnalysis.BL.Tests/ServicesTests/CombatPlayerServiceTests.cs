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
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = CombatPlayerTestDataFactory.CreateDto();
        var entity = CombatPlayerTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICombatPlayerRepository>();

        mockMapper.Setup(m => m.Map<CombatPlayer>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CombatPlayerDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.ResourcesRecovery, result.ResourcesRecovery);
        Assert.Equal(entityDto.DamageDone, result.DamageDone);
        Assert.Equal(entityDto.HealDone, result.HealDone);
        Assert.Equal(entityDto.DamageTaken, result.DamageTaken);
        Assert.Equal(entityDto.CombatId, result.CombatId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatPlayer>(It.IsAny<CombatPlayerDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatPlayer>()), Times.Once);
        mockMapper.Verify(m => m.Map<CombatPlayerDto>(It.IsAny<CombatPlayer>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = CombatPlayerTestDataFactory.CreateDto(username: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICombatPlayerRepository>();

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatPlayer>()), Times.Never);
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

        mockRepository.Setup(m => m.GetByCombatIdAsync(combatPlayerId)).ReturnsAsync(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayerDto>>(entityCollection)).Returns(entityDtoCollection);

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        var result = await service.GetByCombatIdAsync(combatPlayerId);

        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCombatIdAsync(It.IsAny<int>()), Times.Once);
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
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByCombatIdAsync(combatPlayerId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCombatIdAsync(It.IsAny<int>()), Times.Never);
    }
}
