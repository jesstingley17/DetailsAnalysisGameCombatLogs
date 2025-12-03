using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Services;
using CombatAnalysis.Identity.Tests.Factory;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Moq;

namespace CombatAnalysis.Identity.Tests.ServicesTests;

public class UserVerificationServiceTests
{
    [Fact]
    public async Task GenerateResetTokenAsync_ShouldGenerateResetToken()
    {
        // Arrange
        const string email = "testEmail";
        const string token = "token 11 1";

        var entity = ResetTokenTestDataFactory.Create(email: email, token: token);

        var mockRepository = new Mock<IResetTokenRepository>();
        var mockVerifyRepository = new Mock<IVerifyEmailTokenRepository>();
        var mockIdentityRepository = new Mock<IIdentityUserService>();
        var mockTokenRepository = new Mock<IToken>();

        mockTokenRepository.Setup(m => m.GenerateToken()).Returns(token);
        mockRepository.Setup(m => m.CreateAsync(entity)).Returns(Task.CompletedTask);

        var service = new UserVerificationService(mockRepository.Object, mockVerifyRepository.Object, mockIdentityRepository.Object, 
            mockTokenRepository.Object, null);

        // Act
        var result = await service.GenerateResetTokenAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<ResetToken>()), Times.Once);
    }

    [Fact]
    public async Task GenerateVerifyEmailTokenAsync_ShouldGenerateVerifyEmailToken()
    {
        // Arrange
        const string email = "testEmail";
        const string token = "token eka9p";

        var entity = VerifyEmailTokenTestDataFactory.Create(email: email, token: token);

        var mockRepository = new Mock<IResetTokenRepository>();
        var mockVerifyRepository = new Mock<IVerifyEmailTokenRepository>();
        var mockIdentityRepository = new Mock<IIdentityUserService>();
        var mockTokenRepository = new Mock<IToken>();

        mockTokenRepository.Setup(m => m.GenerateToken()).Returns(token);
        mockVerifyRepository.Setup(m => m.CreateAsync(entity)).Returns(Task.CompletedTask);

        var service = new UserVerificationService(mockRepository.Object, mockVerifyRepository.Object, mockIdentityRepository.Object,
            mockTokenRepository.Object, null);

        // Act
        var result = await service.GenerateVerifyEmailTokenAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result);

        // Verify correct method calls
        mockVerifyRepository.Verify(r => r.CreateAsync(It.IsAny<VerifyEmailToken>()), Times.Once);
    }

    [Fact]
    public async Task ResetPasswordAsync_ShouldGenerateVerifyEmailToken()
    {
        // Arrange
        const string identityUserId = "uid-2";
        const int resetTokenId = 1;
        const string email = "testEmail";
        const string token = "token eka9p";
        DateTime expirationTime = DateTime.Parse("11/30/2025");

        var entity = ResetTokenTestDataFactory.Create(id: resetTokenId, email: email, token: token, expirationTime: expirationTime);
        var entityIdentityUserDto = IdentityUserTestDataFactory.CreateDto(id: identityUserId);

        var mockRepository = new Mock<IResetTokenRepository>();
        var mockVerifyRepository = new Mock<IVerifyEmailTokenRepository>();
        var mockIdentityRepository = new Mock<IIdentityUserService>();
        var mockTokenRepository = new Mock<IToken>();

        mockRepository.Setup(m => m.GetByTokenAsync(token)).ReturnsAsync(entity);
        mockIdentityRepository.Setup(m => m.GetByEmailAsync(email)).ReturnsAsync(entityIdentityUserDto);
        mockIdentityRepository.Setup(m => m.UpdateAsync(identityUserId, entityIdentityUserDto)).ReturnsAsync(1);
        mockRepository.Setup(m => m.UpdateAsync(resetTokenId, entity)).ReturnsAsync(1);

        var service = new UserVerificationService(mockRepository.Object, mockVerifyRepository.Object, mockIdentityRepository.Object,
            mockTokenRepository.Object, null);

        // Act
        var result = await service.ResetPasswordAsync(token, email);

        // Assert
        Assert.True(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByTokenAsync(It.IsAny<string>()), Times.Once);
        mockIdentityRepository.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        mockIdentityRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<IdentityUserDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<ResetToken>()), Times.Once);
    }

    [Fact]
    public async Task VerifyEmailAsync_True_ShouldVerifyEmail()
    {
        // Arrange
        const string identityUserId = "uid-2";
        const int tokenId = 1;
        const string email = "testEmail";
        const string token = "token eka9p";
        DateTime expirationTime = DateTime.Parse("11/30/2025");

        var entity = VerifyEmailTokenTestDataFactory.Create(id: tokenId, email: email, token: token, expirationTime: expirationTime);
        var entityIdentityUserDto = IdentityUserTestDataFactory.CreateDto(id: identityUserId, email: email);

        var mockRepository = new Mock<IResetTokenRepository>();
        var mockVerifyRepository = new Mock<IVerifyEmailTokenRepository>();
        var mockIdentityRepository = new Mock<IIdentityUserService>();
        var mockTokenRepository = new Mock<IToken>();

        mockVerifyRepository.Setup(m => m.GetByTokenAsync(token)).ReturnsAsync(entity);
        mockVerifyRepository.Setup(m => m.UpdateAsync(tokenId, entity)).ReturnsAsync(1);
        mockIdentityRepository.Setup(m => m.GetByEmailAsync(email)).ReturnsAsync(entityIdentityUserDto);
        mockIdentityRepository.Setup(m => m.UpdateAsync(identityUserId, entityIdentityUserDto)).ReturnsAsync(1);

        var service = new UserVerificationService(mockRepository.Object, mockVerifyRepository.Object, mockIdentityRepository.Object,
            mockTokenRepository.Object, null);

        // Act
        var result = await service.VerifyEmailAsync(token);

        // Assert
        Assert.True(result);

        // Verify correct method calls
        mockVerifyRepository.Verify(r => r.GetByTokenAsync(It.IsAny<string>()), Times.Once);
        mockVerifyRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<VerifyEmailToken>()), Times.Once);
        mockIdentityRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<IdentityUserDto>()), Times.Once);
    }

    [Fact]
    public async Task RemoveExpiredVerificationAsync_RemoveExpiredVerification()
    {
        // Arrange
        var mockRepository = new Mock<IResetTokenRepository>();
        var mockVerifyRepository = new Mock<IVerifyEmailTokenRepository>();
        var mockIdentityRepository = new Mock<IIdentityUserService>();

        mockVerifyRepository.Setup(m => m.RemoveExpiredVerifyEmailTokenAsync()).Returns(Task.CompletedTask);
        mockRepository.Setup(m => m.RemoveExpiredResetTokenAsync()).Returns(Task.CompletedTask);
        var mockTokenRepository = new Mock<IToken>();

        var service = new UserVerificationService(mockRepository.Object, mockVerifyRepository.Object, mockIdentityRepository.Object,
            mockTokenRepository.Object, null);

        // Act
        await service.RemoveExpiredVerificationAsync();

        // Assert and Verify correct method calls
        mockVerifyRepository.Verify(r => r.RemoveExpiredVerifyEmailTokenAsync(), Times.Once);
        mockRepository.Verify(r => r.RemoveExpiredResetTokenAsync(), Times.Once);
    }
}
