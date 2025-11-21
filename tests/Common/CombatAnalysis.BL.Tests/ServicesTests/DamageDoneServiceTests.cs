using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageDoneServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = DamageDoneTestDataFactory.CreateDto();
        var entity = DamageDoneTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<DamageDone>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<DamageDoneDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Spell, result.Spell);
        Assert.Equal(entityDto.Value, result.Value);
        Assert.Equal(entityDto.Time, result.Time);
        Assert.Equal(entityDto.Creator, result.Creator);
        Assert.Equal(entityDto.Target, result.Target);
        Assert.Equal(entityDto.DamageType, result.DamageType);
        Assert.Equal(entityDto.IsPeriodicDamage, result.IsPeriodicDamage);
        Assert.Equal(entityDto.IsPet, result.IsPet);
        Assert.Equal(entityDto.CombatPlayerId, result.CombatPlayerId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageDone>(It.IsAny<DamageDoneDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageDone>()), Times.Once);
        mockMapper.Verify(m => m.Map<DamageDoneDto>(It.IsAny<DamageDone>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsIdIsZero()
    {
        // Arrange
        var entityDto = DamageDoneTestDataFactory.CreateDto(id: 0);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageDone>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = DamageDoneTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageDone>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = DamageDoneTestDataFactory.CreateDto();
        var entity = DamageDoneTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<DamageDone>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageDone>(It.IsAny<DamageDoneDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageDone>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsIdIsNegative()
    {
        // Arrange
        var entityDto = DamageDoneTestDataFactory.CreateDto(id: -1);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageDone>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = DamageDoneTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(DamageDone.Spell), () => service.UpdateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageDone>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<DamageDone>>();

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
