using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class PlayerDeathServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = PlayerDeathTestDataFactory.CreateDto();
        var entity = PlayerDeathTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        mockMapper.Setup(m => m.Map<CombatPlayerDeath>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CombatPlayerDeathDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Username, result.Username);
        Assert.Equal(entityDto.LastHitSpell, result.LastHitSpell);
        Assert.Equal(entityDto.LastHitValue, result.LastHitValue);
        Assert.Equal(entityDto.Time, result.Time);
        Assert.Equal(entityDto.CombatPlayerId, result.CombatPlayerId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatPlayerDeath>(It.IsAny<CombatPlayerDeathDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatPlayerDeath>()), Times.Once);
        mockMapper.Verify(m => m.Map<CombatPlayerDeathDto>(It.IsAny<CombatPlayerDeath>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = PlayerDeathTestDataFactory.CreateDto(username: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatPlayerDeath>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = PlayerDeathTestDataFactory.CreateDto();
        var entity = PlayerDeathTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        mockMapper.Setup(m => m.Map<CombatPlayerDeath>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatPlayerDeath>(It.IsAny<CombatPlayerDeathDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CombatPlayerDeath>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = PlayerDeathTestDataFactory.CreateDto(username: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CombatPlayerDeath.Username), () => service.UpdateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CombatPlayerDeath>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act
        var entityDeleted = await service.DeleteAsync(id);

        // Assert
        Assert.True(entityDeleted);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_False_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 2;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act
        var entityDeleted = await service.DeleteAsync(id);

        // Assert
        Assert.False(entityDeleted);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowArgumentOutOfRangeException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<CombatPlayerDeath>>();

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
