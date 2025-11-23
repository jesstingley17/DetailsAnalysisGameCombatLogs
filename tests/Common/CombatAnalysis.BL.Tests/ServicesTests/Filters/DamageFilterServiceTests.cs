using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services.Filters;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests.Filters;

public class DamageFilterServiceTests
{
    [Fact]
    public async Task GetDamageByEachTargetAsync_CollectionOfEntity_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatId = 1;

        var combatTargetsDto = new List<List<CombatTargetDto>> { CombatTargetTestDataFactory.CreateDtoCollection() };
        var combatTargets = new List<List<CombatTarget>> { CombatTargetTestDataFactory.CreateCollection() };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IDamageFilterRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<List<CombatTargetDto>>>(combatTargets)).Returns(combatTargetsDto);

        mockRepository.Setup(m => m.GetDamageByEachTargetAsync(combatId)).ReturnsAsync(combatTargets);

        var service = new DamageFilterService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetDamageByEachTargetAsync(combatId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.NotNull(result.First());
        Assert.NotEmpty(result.First());
        Assert.Equal(3, result.First().Count);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetDamageByEachTargetAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetDamageByEachTargetAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatId = 0;

        var combatTargetsDto = new List<List<CombatTargetDto>> { CombatTargetTestDataFactory.CreateDtoCollection() };
        var combatTargets = new List<List<CombatTarget>> { CombatTargetTestDataFactory.CreateCollection() };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IDamageFilterRepository>();

        mockMapper.Setup(m => m.Map<IEnumerable<List<CombatTargetDto>>>(combatTargets)).Returns(combatTargetsDto);

        mockRepository.Setup(m => m.GetDamageByEachTargetAsync(combatId)).ReturnsAsync(combatTargets);

        var service = new DamageFilterService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetDamageByEachTargetAsync(combatId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetDamageByEachTargetAsync(It.IsAny<int>()), Times.Never);
    }
}
