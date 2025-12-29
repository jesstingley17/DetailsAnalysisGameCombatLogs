using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageDoneGeneralServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityGeneralDto = DamageDoneGeneralTestDataFactory.CreateDto();
        var entity = DamageDoneGeneralTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        mockMapper.Setup(m => m.Map<DamageDoneGeneral>(entityGeneralDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<DamageDoneGeneralDto>(entity)).Returns(entityGeneralDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityGeneralDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityGeneralDto.Id, result.Id);
        Assert.Equal(entityGeneralDto.Spell, result.Spell);
        Assert.Equal(entityGeneralDto.Value, result.Value);
        Assert.Equal(entityGeneralDto.DamagePerSecond, result.DamagePerSecond);
        Assert.Equal(entityGeneralDto.CritNumber, result.CritNumber);
        Assert.Equal(entityGeneralDto.MissNumber, result.MissNumber);
        Assert.Equal(entityGeneralDto.CastNumber, result.CastNumber);
        Assert.Equal(entityGeneralDto.MinValue, result.MinValue);
        Assert.Equal(entityGeneralDto.MaxValue, result.MaxValue);
        Assert.Equal(entityGeneralDto.AverageValue, result.AverageValue);
        Assert.Equal(entityGeneralDto.IsPet, result.IsPet);
        Assert.Equal(entityGeneralDto.CombatPlayerId, result.CombatPlayerId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageDoneGeneral>(It.IsAny<DamageDoneGeneralDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageDoneGeneral>()), Times.Once);
        mockMapper.Verify(m => m.Map<DamageDoneGeneralDto>(It.IsAny<DamageDoneGeneral>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityGeneralDto = DamageDoneGeneralTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityGeneralDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageDoneGeneral>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityGeneralDto = DamageDoneGeneralTestDataFactory.CreateDto();
        var entity = DamageDoneGeneralTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        mockMapper.Setup(m => m.Map<DamageDoneGeneral>(entityGeneralDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityGeneralDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageDoneGeneral>(It.IsAny<DamageDoneGeneralDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageDoneGeneral>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityGeneralDto = DamageDoneGeneralTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(DamageDoneGeneral.Spell), () => service.UpdateAsync(entityGeneralDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageDoneGeneral>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageDoneGeneral>>();

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
