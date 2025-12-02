using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto();
        var entity = CombatTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<Combat>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CombatDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.LocallyNumber, result.LocallyNumber);
        Assert.Equal(entityDto.DungeonName, result.DungeonName);
        Assert.Equal(entityDto.Name, result.Name);
        Assert.Equal(entityDto.Difficulty, result.Difficulty);
        Assert.Equal(entityDto.DamageDone, result.DamageDone);
        Assert.Equal(entityDto.HealDone, result.HealDone);
        Assert.Equal(entityDto.DamageTaken, result.DamageTaken);
        Assert.Equal(entityDto.EnergyRecovery, result.EnergyRecovery);
        Assert.Equal(entityDto.IsWin, result.IsWin);
        Assert.Equal(entityDto.StartDate, result.StartDate);
        Assert.Equal(entityDto.FinishDate, result.FinishDate);
        Assert.Equal(entityDto.Duration, result.Duration);
        Assert.Equal(entityDto.IsReady, result.IsReady);
        Assert.Equal(entityDto.CombatLogId, result.CombatLogId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<Combat>(It.IsAny<CombatDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Combat>()), Times.Once);
        mockMapper.Verify(m => m.Map<CombatDto>(It.IsAny<Combat>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto(difficulty: -1);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Combat>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto();
        var entity = CombatTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockMapper.Setup(m => m.Map<Combat>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<Combat>(It.IsAny<CombatDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Combat>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto(difficulty: -1);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(Combat.Difficulty), () => service.UpdateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Combat>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<Combat>>();

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
