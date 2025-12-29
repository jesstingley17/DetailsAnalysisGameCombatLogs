using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class SpecializationScoreServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDto = SpecializationScoreTestDataFactory.CreateDto();
        var entity = SpecializationScoreTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        mockMapper.Setup(m => m.Map<SpecializationScore>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<SpecializationScoreDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).ReturnsAsync(entity);

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(entityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.SpecId, result.SpecId);
        Assert.Equal(entityDto.BossId, result.BossId);
        Assert.Equal(entityDto.Damage, result.Damage);
        Assert.Equal(entityDto.Heal, result.Heal);
        Assert.Equal(entityDto.Updated, result.Updated);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<SpecializationScore>(It.IsAny<SpecializationScoreDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<SpecializationScore>()), Times.Once);
        mockMapper.Verify(m => m.Map<SpecializationScoreDto>(It.IsAny<SpecializationScore>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentOutOfRangeException_ShouldNotCreateEntityAsSomeParamsIncorrect()
    {
        // Arrange
        var entityDto = SpecializationScoreTestDataFactory.CreateDto(damage: -1);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<SpecializationScore>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = SpecializationScoreTestDataFactory.CreateDto();
        var entity = SpecializationScoreTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        mockMapper.Setup(m => m.Map<SpecializationScore>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity));

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<SpecializationScore>(It.IsAny<SpecializationScoreDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<SpecializationScore>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = SpecializationScoreTestDataFactory.CreateDto(damage: -1);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(SpecializationScore.Damage), () => service.UpdateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<SpecializationScore>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_True_ShouldDeleteEntity()
    {
        // Arrange
        const int id = 1;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

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
        var mockRepository = new Mock<IGenericRepositoryBatch<SpecializationScore>>();
        var mockSpecRepository = new Mock<ISpecScore>();

        var service = new SpecializationScoreService(mockSpecRepository.Object, mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
