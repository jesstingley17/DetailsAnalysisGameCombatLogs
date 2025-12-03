using CombatAnalysis.Identity.Services;
using CombatAnalysis.IdentityDAL.Interfaces;
using Moq;

namespace CombatAnalysis.Identity.Tests.ServicesTests;

public class AuthCodeServiceTests
{
    [Fact]
    public async Task RemoveExpiredCodesAsync_ShouldRemoveExpiredCodes()
    {
        // Arrange
        var mockRepository = new Mock<IPkeRepository>();

        mockRepository.Setup(m => m.RemoveExpiredCodesAsync()).Returns(Task.CompletedTask);

        var service = new AuthCodeService(mockRepository.Object);

        // Act
        await service.RemoveExpiredCodesAsync();

        // Assert and Verify correct method calls
        mockRepository.Verify(r => r.RemoveExpiredCodesAsync(), Times.Once);
    }
}
