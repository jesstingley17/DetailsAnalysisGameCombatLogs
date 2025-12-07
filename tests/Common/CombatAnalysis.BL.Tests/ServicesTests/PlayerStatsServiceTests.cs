using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class PlayerStatsServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = PlayerStatsTestDataFactory.CreateDto();
        var entity = PlayerStatsTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        mockMapper.Setup(m => m.Map<PlayerStats>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<PlayerStatsDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Faction, result.Faction);
        Assert.Equal(entityDto.Strength, result.Strength);
        Assert.Equal(entityDto.Agility, result.Agility);
        Assert.Equal(entityDto.Intelligence, result.Intelligence);
        Assert.Equal(entityDto.Stamina, result.Stamina);
        Assert.Equal(entityDto.Spirit, result.Spirit);
        Assert.Equal(entityDto.Dodge, result.Dodge);
        Assert.Equal(entityDto.Parry, result.Parry);
        Assert.Equal(entityDto.Crit, result.Crit);
        Assert.Equal(entityDto.Haste, result.Haste);
        Assert.Equal(entityDto.Hit, result.Hit);
        Assert.Equal(entityDto.Expertise, result.Expertise);
        Assert.Equal(entityDto.Armor, result.Armor);
        Assert.Equal(entityDto.Talents, result.Talents);
        Assert.Equal(entityDto.CombatPlayerId, result.CombatPlayerId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<PlayerStats>(It.IsAny<PlayerStatsDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<PlayerStats>()), Times.Once);
        mockMapper.Verify(m => m.Map<PlayerStatsDto>(It.IsAny<PlayerStats>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = PlayerStatsTestDataFactory.CreateDto(talents: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<PlayerStats>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = PlayerStatsTestDataFactory.CreateDto();
        var entity = PlayerStatsTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        mockMapper.Setup(m => m.Map<PlayerStats>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<PlayerStats>(It.IsAny<PlayerStatsDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<PlayerStats>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = PlayerStatsTestDataFactory.CreateDto(talents: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(PlayerStats.Talents), () => service.UpdateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<PlayerStats>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<PlayerStats>>();

        var service = new PlayerStatsService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
