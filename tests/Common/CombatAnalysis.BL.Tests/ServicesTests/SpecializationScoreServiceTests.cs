using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class SpecializationScoreServiceTests
{
    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entityDto = SpecializationScoreTestDataFactory.CreateDto();
        var entity = SpecializationScoreTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ISpecializationScoreRepository>();

        mockMapper.Setup(m => m.Map<SpecializationScore>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(entity, CancellationToken.None));

        var service = new SpecializationScoreService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(entityDto, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<SpecializationScore>(It.IsAny<SpecializationScoreDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<SpecializationScore>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowException_ShouldNotUpdateEntityAsDifficultyIsNegative()
    {
        // Arrange
        var entityDto = SpecializationScoreTestDataFactory.CreateDto(damage: -1);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ISpecializationScoreRepository>();

        var service = new SpecializationScoreService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(SpecializationScore.DamageScore), () => service.UpdateAsync(entityDto, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<SpecializationScore>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
