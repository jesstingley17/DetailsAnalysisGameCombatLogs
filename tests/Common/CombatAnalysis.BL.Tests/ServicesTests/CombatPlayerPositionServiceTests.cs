using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatPlayerPositionServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = CombatPlayerPositionTestDataFactory.CreateDto();
        var entity = CombatPlayerPositionTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CombatPlayerPosition>>();

        mockMapper.Setup(m => m.Map<CombatPlayerPosition>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<CombatPlayerPositionDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new CombatPlayerPositionService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.PositionX, result.PositionX);
        Assert.Equal(entityDto.PositionY, result.PositionY);
        Assert.Equal(entityDto.CombatId, result.CombatId);
        Assert.Equal(entityDto.CombatPlayerId, result.CombatPlayerId);
        Assert.Equal(entityDto.CombatId, result.CombatId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatPlayerPosition>(It.IsAny<CombatPlayerPositionDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<CombatPlayerPosition>()), Times.Once);
        mockMapper.Verify(m => m.Map<CombatPlayerPositionDto>(It.IsAny<CombatPlayerPosition>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = CombatPlayerPositionTestDataFactory.CreateDto();
        var entity = CombatPlayerPositionTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CombatPlayerPosition>>();

        mockMapper.Setup(m => m.Map<CombatPlayerPosition>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new CombatPlayerPositionService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<CombatPlayerPosition>(It.IsAny<CombatPlayerPositionDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<CombatPlayerPosition>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<CombatPlayerPosition>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new CombatPlayerPositionService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CombatPlayerPosition>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new CombatPlayerPositionService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<CombatPlayerPosition>>();

        var service = new CombatPlayerPositionService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
