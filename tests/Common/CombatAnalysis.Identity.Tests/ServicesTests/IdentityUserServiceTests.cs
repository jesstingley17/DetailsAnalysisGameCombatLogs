using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Services;
using CombatAnalysis.Identity.Tests.Factory;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Moq;

namespace CombatAnalysis.Identity.Tests.ServicesTests;

public class IdentityUserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateEntity()
    {
        // Arrange
        var entityDto = IdentityUserTestDataFactory.CreateDto();
        var entity = IdentityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockMapper.Setup(m => m.Map<IdentityUser>(entityDto)).Returns(entity);
        mockMapper.Setup(m => m.Map<IdentityUserDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.CreateAsync(entity)).Returns(Task.CompletedTask);
        mockRepository.Setup(m => m.GetByIdAsync(entityDto.Id)).ReturnsAsync(entity);

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateAsync(entityDto);

        var result = await service.GetByIdAsync(entityDto.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityDto.Id, result.Id);
        Assert.Equal(entityDto.Email, result.Email);
        Assert.Equal(entityDto.PasswordHash, result.PasswordHash);
        Assert.Equal(entityDto.Salt, result.Salt);
        Assert.Equal(entityDto.EmailVerified, result.EmailVerified);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<IdentityUser>(It.IsAny<IdentityUserDto>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<IdentityUser>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ThrowException_ShouldNotCreateEntityAsInitiatorIdEmpty()
    {
        // Arrange
        const string id = "";

        var entityDto = IdentityUserTestDataFactory.CreateDto(id: id);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(IdentityUserDto.Id), () => service.CreateAsync(entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<IdentityUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        const string id = "uid-1";
        const string email = "newEmail";

        var entityDto = IdentityUserTestDataFactory.CreateDto(id: id, email: email);
        var entity = IdentityUserTestDataFactory.Create(id: id, email: email);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockMapper.Setup(m => m.Map<IdentityUser>(entityDto)).Returns(entity);

        mockRepository.Setup(m => m.UpdateAsync(id, It.IsAny<IdentityUser>()));

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(id, entityDto);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IdentityUser>(It.IsAny<IdentityUserDto>()), Times.Once);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowArgumentException_ShouldNotUpdateEntityAsEmailEmpty()
    {
        // Arrange
        const string id = "uid-1";
        const string email = "";

        var entityDto = IdentityUserTestDataFactory.CreateDto(id: id, email: email);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(id, entityDto));

        // Verify correct method calls
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_OneEntity_ShouldReturnOneEntity()
    {
        // Arrange
        const string id = "uid-1-1";

        var entityDto = IdentityUserTestDataFactory.CreateDto(id: id);
        var entity = IdentityUserTestDataFactory.Create(id: id);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockMapper.Setup(m => m.Map<IdentityUserDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(entity);

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NoAnyEntity_ShouldReturnNoAnyEntity()
    {
        // Arrange
        const string id = "uid-1-1-2";

        var notificationDto = IdentityUserTestDataFactory.CreateDto();
        var notification = IdentityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockMapper.Setup(m => m.Map<IdentityUserDto>(notification)).Returns(notificationDto);

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowArgumentException_ShouldNotReturnAnyEntity()
    {
        // Arrange
        const string id = "";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetByIdAsync(id));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task CheckByEmailAsync_True_ShouldCheckIfEmailAlreadyUsed()
    {
        // Arrange
        const string email = "email-1";

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockRepository.Setup(m => m.CheckByEmailAsync(email)).ReturnsAsync(true);

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CheckByEmailAsync(email);

        // Assert
        Assert.True(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CheckByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CheckByEmailAsync_False_ShouldCheckIfEmailAlreadyUsed()
    {
        // Arrange
        const string email = "email-1-23";

        var notificationDto = IdentityUserTestDataFactory.CreateDto();
        var notification = IdentityUserTestDataFactory.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockMapper.Setup(m => m.Map<IdentityUserDto>(notification)).Returns(notificationDto);

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CheckByEmailAsync(email);

        // Assert
        Assert.False(result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CheckByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetByEmailAsync_Entity_ShouldReturnEntityByEmail()
    {
        // Arrange
        const string email = "email-1";

        var entityDto = IdentityUserTestDataFactory.CreateDto(email: email);
        var entity = IdentityUserTestDataFactory.Create(email: email);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IIdentityUserRepository>();

        mockMapper.Setup(m => m.Map<IdentityUserDto>(entity)).Returns(entityDto);

        mockRepository.Setup(m => m.GetAsync(email)).ReturnsAsync(entity);

        var service = new IdentityUserService(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAsync(It.IsAny<string>()), Times.Once);
    }
}
