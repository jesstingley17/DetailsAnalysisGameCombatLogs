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
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        mockMapper.Setup(m => m.Map<Combat>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CombatDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity, CancellationToken.None)).ReturnsAsync(entity);

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.DungeonName, result.DungeonName);
        Assert.Equal(entityDto.DamageDone, result.DamageDone);
        Assert.Equal(entityDto.HealDone, result.HealDone);
        Assert.Equal(entityDto.DamageTaken, result.DamageTaken);
        Assert.Equal(entityDto.ResourcesRecovery, result.ResourcesRecovery);
        Assert.Equal(entityDto.IsWin, result.IsWin);
        Assert.Equal(entityDto.StartDate, result.StartDate);
        Assert.Equal(entityDto.FinishDate, result.FinishDate);
        Assert.Equal(entityDto.Duration, result.Duration);
        Assert.Equal(entityDto.IsReady, result.IsReady);
        Assert.Equal(entityDto.CombatLogId, result.CombatLogId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<Combat>(It.IsAny<CombatDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Combat>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<CombatDto>(It.IsAny<Combat>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto(dungeonName: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CreateAsync(entityDto, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<Combat>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto();
        var entity = CombatTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        mockMapper.Setup(m => m.Map<Combat>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity, CancellationToken.None));

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<Combat>(It.IsAny<CombatDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Combat>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = CombatTestDataFactory.CreateDto(dungeonName: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(Combat.DungeonName), () => service.UpdateAsync(entityDto, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Combat>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        mockRepository.Setup(r => r.DeleteAsync(id, CancellationToken.None)).ReturnsAsync(true);

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act
        var entityDeleted = await service.DeleteAsync(id, CancellationToken.None);

        // Assert
        Assert.True(entityDeleted);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_False_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 2;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        mockRepository.Setup(r => r.DeleteAsync(id, CancellationToken.None)).ReturnsAsync(false);

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act
        var entityDeleted = await service.DeleteAsync(id, CancellationToken.None);

        // Assert
        Assert.False(entityDeleted);

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowArgumentOutOfRangeException_ShouldNotDeleteEntity()
    {
        // Arrange
        const int id = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<Combat>>();

        var service = new CombatService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
