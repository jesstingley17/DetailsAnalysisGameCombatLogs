using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageTakenServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityTakenDto = DamageTakenTestDataFactory.CreateDto();
        var entity = DamageTakenTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        mockMapper.Setup(m => m.Map<DamageTaken>(entityTakenDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<DamageTakenDto>(entity)).Returns(entityTakenDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityTakenDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityTakenDto.Id, result.Id);
        Assert.Equal(entityTakenDto.Spell, result.Spell);
        Assert.Equal(entityTakenDto.Value, result.Value);
        Assert.Equal(entityTakenDto.Time, result.Time);
        Assert.Equal(entityTakenDto.Creator, result.Creator);
        Assert.Equal(entityTakenDto.Target, result.Target);
        Assert.Equal(entityTakenDto.DamageTakenType, result.DamageTakenType);
        Assert.Equal(entityTakenDto.IsPeriodicDamage, result.IsPeriodicDamage);
        Assert.Equal(entityTakenDto.Resisted, result.Resisted);
        Assert.Equal(entityTakenDto.Absorbed, result.Absorbed);
        Assert.Equal(entityTakenDto.Blocked, result.Blocked);
        Assert.Equal(entityTakenDto.RealDamage, result.RealDamage);
        Assert.Equal(entityTakenDto.Mitigated, result.Mitigated);
        Assert.Equal(entityTakenDto.CombatPlayerId, result.CombatPlayerId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageTaken>(It.IsAny<DamageTakenDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageTaken>()), Times.Once);
        mockMapper.Verify(m => m.Map<DamageTakenDto>(It.IsAny<DamageTaken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityTakenDto = DamageTakenTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityTakenDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageTaken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityTakenDto = DamageTakenTestDataFactory.CreateDto();
        var entity = DamageTakenTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        mockMapper.Setup(m => m.Map<DamageTaken>(entityTakenDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityTakenDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageTaken>(It.IsAny<DamageTakenDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageTaken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityTakenDto = DamageTakenTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(DamageTaken.Spell), () => service.UpdateAsync(entityTakenDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageTaken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepository<DamageTaken>>();

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
