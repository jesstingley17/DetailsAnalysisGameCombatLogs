using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
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
        var mockRepository = new Mock<IGenericRepository<CombatPlayer>>();

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
        var mockRepository = new Mock<IGenericRepository<CombatPlayer>>();

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatPlayer>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = CombatPlayerTestDataFactory.CreateDto();
        var entity = CombatPlayerTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CombatPlayer>>();

        mockMapper.Setup(m => m.Map<CombatPlayer>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatPlayer>(It.IsAny<CombatPlayerDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CombatPlayer>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CombatPlayer>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CombatPlayer>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CombatPlayer>>();

        var service = new CombatPlayerService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
