using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatLogServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = CombatLogTestDataFactory.CreateDto();
        var entity = CombatLogTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        mockMapper.Setup(m => m.Map<CombatLog>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CombatLogDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity, CancellationToken.None)).ReturnsAsync(entity);

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Name, result.Name);
        Assert.Equal(entityDto.Date, result.Date);
        Assert.Equal(entityDto.LogType, result.LogType);
        Assert.Equal(entityDto.NumberReadyCombats, result.NumberReadyCombats);
        Assert.Equal(entityDto.CombatsInQueue, result.CombatsInQueue);
        Assert.Equal(entityDto.IsReady, result.IsReady);
        Assert.Equal(entityDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatLog>(It.IsAny<CombatLogDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatLog>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<CombatLogDto>(It.IsAny<CombatLog>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = CombatLogTestDataFactory.CreateDto(name: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityDto, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatLog>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = CombatLogTestDataFactory.CreateDto();
        var entity = CombatLogTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        mockMapper.Setup(m => m.Map<CombatLog>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity, CancellationToken.None));

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatLog>(It.IsAny<CombatLogDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CombatLog>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = CombatLogTestDataFactory.CreateDto(name: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(CombatLog.Name), () => service.UpdateAsync(entityDto, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CombatLog>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        mockRepository.Setup(r => r.DeleteAsync(id, CancellationToken.None)).ReturnsAsync(true);

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        mockRepository.Setup(r => r.DeleteAsync(id, CancellationToken.None)).ReturnsAsync(false);

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<ICreateEntityRepository<CombatLog>>();

        var service = new CombatLogService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
