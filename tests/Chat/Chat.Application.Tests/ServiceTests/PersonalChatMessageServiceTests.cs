using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Services;
using Chat.Application.Tests.Factory;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Moq;

namespace Chat.Application.Tests.ServiceTests;

public class PersonalChatMessageServiceTests
{
    [Fact]
    public async Task CreateAsync_PersonalChatMessage_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var personalChat = PersonalChatTestData.Create();
        var personalChatMessageDto = PersonalChatMessageTestData.CreateDto();
        var personalChatMessage = PersonalChatMessageTestData.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>())).Returns(personalChatMessageDto);

        mockChatRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatId>())).ReturnsAsync(personalChat);
        mockRepository.Setup(m => m.CreateAsync(It.IsAny<PersonalChatMessage>())).ReturnsAsync(personalChatMessage);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(personalChatMessageDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personalChatMessageDto.Id, result.Id);
        Assert.Equal(personalChatMessageDto.Username, result.Username);
        Assert.Equal(personalChatMessageDto.Message, result.Message);
        Assert.Equal(personalChatMessageDto.Time, result.Time);
        Assert.Equal(personalChatMessageDto.Status, result.Status);
        Assert.Equal(personalChatMessageDto.Type, result.Type);
        Assert.Equal(personalChatMessageDto.MarkedType, result.MarkedType);
        Assert.Equal(personalChatMessageDto.PersonalChatId, result.PersonalChatId);
        Assert.Equal(personalChatMessageDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<PersonalChatMessage>()), Times.Once);
        mockMapper.Verify(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChatAsync_ShouldUpdateChatMessageProperty()
    {
        // Arrange
        PersonalChatMessageId id = 1;
        const string message = "updated message";
        const MessageStatus status = MessageStatus.Sent;
        const MessageMarkedType type = MessageMarkedType.None;

        var personalChatDto = PersonalChatMessageTestData.CreateDto(status: status, markedType: type);
        var personalChat = PersonalChatMessageTestData.Create(status: status, markedType: type);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>())).Returns(personalChatDto);

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatMessageId>())).ReturnsAsync(personalChat);
        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateChatMessageAsync(id, message, null, null);

        // Assert
        Assert.Equal(message, personalChat.Message);
        Assert.Equal(status, personalChat.Status);
        Assert.Equal(type, personalChat.MarkedType);

        // Verify correct method calls
        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateChatAsync_ShouldUpdateChatMessageStatusProperty()
    {
        // Arrange
        PersonalChatMessageId id = 1;
        const string message = "original message";
        const MessageStatus status = MessageStatus.Read;
        const MessageMarkedType type = MessageMarkedType.None;

        var personalChatDto = PersonalChatMessageTestData.CreateDto(message: message, markedType: type);
        var personalChat = PersonalChatMessageTestData.Create(message: message, markedType: type);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>())).Returns(personalChatDto);

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatMessageId>())).ReturnsAsync(personalChat);
        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateChatMessageAsync(id, null, status, null);

        // Assert
        Assert.Equal(message, personalChat.Message);
        Assert.Equal(status, personalChat.Status);
        Assert.Equal(type, personalChat.MarkedType);

        // Verify correct method calls
        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateChatAsync_ShouldUpdateChatMessageMarkedTypeProperty()
    {
        // Arrange
        PersonalChatMessageId id = 1;
        const string message = "original message";
        const MessageStatus status = MessageStatus.Sent;
        const MessageMarkedType type = MessageMarkedType.NotReleveant;

        var personalChatDto = PersonalChatMessageTestData.CreateDto(message: message, status: status);
        var personalChat = PersonalChatMessageTestData.Create(message: message, status: status);

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>())).Returns(personalChatDto);

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatMessageId>())).ReturnsAsync(personalChat);
        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateChatMessageAsync(id, null, null, type);

        // Assert
        Assert.Equal(message, personalChat.Message);
        Assert.Equal(status, personalChat.Status);
        Assert.Equal(type, personalChat.MarkedType);

        // Verify correct method calls
        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        const int chatId = 1;

        var personalChatsDto = PersonalChatMessageTestData.CreateDtoCollection();
        var personalChats = PersonalChatMessageTestData.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(chatId);

        // Assert and Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<PersonalChatMessageId>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowEntityNotFoundException_ShouldNotRemoveEntityAsEntityDoesNotExist()
    {
        // Arrange
        const int chatId = 0;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<PersonalChatMessageId>())).Throws(new EntityNotFoundException(typeof(PersonalChatMessage), chatId));

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(chatId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<PersonalChatMessageId>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
    {
        // Arrange
        var personalChatsDto = PersonalChatMessageTestData.CreateDtoCollection();
        var personalChats = PersonalChatMessageTestData.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<IEnumerable<PersonalChatMessageDto>>(It.IsAny<IEnumerable<PersonalChatMessage>>())).Returns(personalChatsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(personalChats);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<PersonalChatMessageDto>>(It.IsAny<IEnumerable<PersonalChatMessage>>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_PersonalChatMessage_ShouldReturnCorrectEntity()
    {
        // Arrange
        const int chatId = 1;

        var personalChatDto = PersonalChatMessageTestData.CreateDto();
        var personalChat = PersonalChatMessageTestData.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>())).Returns(personalChatDto);

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<PersonalChatMessageId>())).ReturnsAsync(personalChat);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(chatId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personalChatDto.Id, result.Id);
        Assert.Equal(personalChatDto.Username, result.Username);
        Assert.Equal(personalChatDto.Message, result.Message);
        Assert.Equal(personalChatDto.Time, result.Time);
        Assert.Equal(personalChatDto.Status, result.Status);
        Assert.Equal(personalChatDto.Type, result.Type);
        Assert.Equal(personalChatDto.MarkedType, result.MarkedType);
        Assert.Equal(personalChatDto.PersonalChatId, result.PersonalChatId);
        Assert.Equal(personalChatDto.AppUserId, result.AppUserId);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<PersonalChatMessageId>()), Times.Once);
        mockMapper.Verify(m => m.Map<PersonalChatMessageDto>(It.IsAny<PersonalChatMessage>()), Times.Once);
    }

    [Fact]
    public async Task GetByChatIdAsync_Collection_ShouldReturnCollectionByChatId()
    {
        // Arrange
        const int chatId = 1;
        const int page = 1;
        const int pageSize = 2;

        var personalChatsDto = PersonalChatMessageTestData.CreateDtoCollection();
        var personalChats = PersonalChatMessageTestData.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockMapper.Setup(m => m.Map<IEnumerable<PersonalChatMessageDto>>(It.IsAny<IEnumerable<PersonalChatMessage>>())).Returns(personalChatsDto);

        mockRepository.Setup(m => m.GetByChatIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(personalChats);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByChatIdAsync(chatId, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByChatIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<PersonalChatMessageDto>>(It.IsAny<IEnumerable<PersonalChatMessage>>()), Times.Once);
    }

    [Fact]
    public async Task CountByChatIdAsync_Count_ShouldReturnCountEntityByChatId()
    {
        // Arrange
        const int chatId = 1;
        const int count = 1;

        var personalChatsDto = PersonalChatMessageTestData.CreateDtoCollection();
        var personalChats = PersonalChatMessageTestData.CreateCollection();

        var mockRepository = new Mock<IPersonalChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<PersonalChat, PersonalChatId>>();

        mockRepository.Setup(m => m.CountByChatIdAsync(It.IsAny<int>())).ReturnsAsync(count);

        var service = new PersonalChatMessageService(mockRepository.Object, mockChatRepository.Object, null);

        // Act
        var result = await service.CountByChatIdAsync(chatId);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByChatIdAsync(It.IsAny<int>()), Times.Once);
    }
}
