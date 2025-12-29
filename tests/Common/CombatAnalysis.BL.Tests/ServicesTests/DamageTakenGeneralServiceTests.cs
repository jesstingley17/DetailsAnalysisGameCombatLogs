using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageTakenGeneralServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityTakenDto = DamageTakenGeneralTestDataFactory.CreateDto();
        var entity = DamageTakenGeneralTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        mockMapper.Setup(m => m.Map<DamageTakenGeneral>(entityTakenDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<DamageTakenGeneralDto>(entity)).Returns(entityTakenDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityTakenDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityTakenDto.Id, result.Id);
        Assert.Equal(entityTakenDto.Spell, result.Spell);
        Assert.Equal(entityTakenDto.Value, result.Value);
        Assert.Equal(entityTakenDto.ActualValue, result.ActualValue);
        Assert.Equal(entityTakenDto.DamageTakenPerSecond, result.DamageTakenPerSecond);
        Assert.Equal(entityTakenDto.CritNumber, result.CritNumber);
        Assert.Equal(entityTakenDto.MissNumber, result.MissNumber);
        Assert.Equal(entityTakenDto.CastNumber, result.CastNumber);
        Assert.Equal(entityTakenDto.MinValue, result.MinValue);
        Assert.Equal(entityTakenDto.MaxValue, result.MaxValue);
        Assert.Equal(entityTakenDto.AverageValue, result.AverageValue);
        Assert.Equal(entityTakenDto.CombatPlayerId, result.CombatPlayerId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageTakenGeneral>(It.IsAny<DamageTakenGeneralDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageTakenGeneral>()), Times.Once);
        mockMapper.Verify(m => m.Map<DamageTakenGeneralDto>(It.IsAny<DamageTakenGeneral>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityTakenDto = DamageTakenGeneralTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(entityTakenDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<DamageTakenGeneral>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityTakenDto = DamageTakenGeneralTestDataFactory.CreateDto();
        var entity = DamageTakenGeneralTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        mockMapper.Setup(m => m.Map<DamageTakenGeneral>(entityTakenDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityTakenDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<DamageTakenGeneral>(It.IsAny<DamageTakenGeneralDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageTakenGeneral>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityTakenDto = DamageTakenGeneralTestDataFactory.CreateDto(spell: "");

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(DamageTakenGeneral.Spell), () => service.UpdateAsync(entityTakenDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<DamageTakenGeneral>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepositoryBatch<DamageTakenGeneral>>();

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
