using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests.General;

public class CountServiceTests
{
    [Fact]
    public async Task CountByCombatPlayerIdAsync_CountOfEntity_ShouldCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 1;
        const int count = 10;

        var mockRepository = new Mock<ICountRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None)).ReturnsAsync(count);

        var service = new CountService<DamageDoneDto, DamageDone>(mockRepository.Object);

        // Act
        var result = await service.CountByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CountByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 0;
        const int count = 10;

        var mockRepository = new Mock<ICountRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None)).ReturnsAsync(count);

        var service = new CountService<DamageDoneDto, DamageDone>(mockRepository.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CountByCombatPlayerIdAsync(combatPlayerId, CancellationToken.None));

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
