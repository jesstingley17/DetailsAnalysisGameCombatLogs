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

public class GroupChatMessageServiceTests
{
    [Fact]
    public async Task CreateAsync_GroupChatMessage_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        GroupChatId chatId = 1;

        var groupChat = GroupChatTestData.Create(id: chatId);
        var groupChatUser = GroupChatUserTestData.Create(chatId: chatId);
        var groupChatMessageDto = GroupChatMessageTestData.CreateDto();
        var groupChatMessage = GroupChatMessageTestData.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockMapper.Setup(m => m.Map<GroupChatMessageDto>(It.IsAny<GroupChatMessage>())).Returns(groupChatMessageDto);

        mockChatRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatId>())).ReturnsAsync(groupChat);
        mockChatUserRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatUserId>())).ReturnsAsync(groupChatUser);
        mockRepository.Setup(m => m.CreateAsync(It.IsAny<GroupChatMessage>())).ReturnsAsync(groupChatMessage);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CreateAsync(groupChatMessageDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(groupChatMessageDto.Id, result.Id);
        Assert.Equal(groupChatMessageDto.Username, result.Username);
        Assert.Equal(groupChatMessageDto.Message, result.Message);
        Assert.Equal(groupChatMessageDto.Time, result.Time);
        Assert.Equal(groupChatMessageDto.Status, result.Status);
        Assert.Equal(groupChatMessageDto.Type, result.Type);
        Assert.Equal(groupChatMessageDto.MarkedType, result.MarkedType);
        Assert.Equal(groupChatMessageDto.IsEdited, result.IsEdited);
        Assert.Equal(groupChatMessageDto.GroupChatId, result.GroupChatId);
        Assert.Equal(groupChatMessageDto.GroupChatUserId, result.GroupChatUserId);

        // Verify correct method calls
        mockMapper.Verify(m => m.Map<GroupChatMessageDto>(It.IsAny<GroupChatMessage>()), Times.Once);

        mockChatRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatId>()), Times.Once);
        mockChatUserRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatUserId>()), Times.Once);
        mockRepository.Verify(r => r.CreateAsync(It.IsAny<GroupChatMessage>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChatMessageAsync_ShouldUpdateChatMessageProperty()
    {
        // Arrange
        GroupChatMessageId id = 1;
        const string message = "updated message";
        const MessageStatus status = MessageStatus.Sent;
        const MessageMarkedType type = MessageMarkedType.None;

        var groupChatDto = GroupChatMessageTestData.CreateDto(status: status, markedType: type);
        var groupChat = GroupChatMessageTestData.Create(status: status, markedType: type);

        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatMessageId>())).ReturnsAsync(groupChat);
        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, null);

        // Act
        await service.UpdateChatMessageAsync(id, message, null, null);

        // Assert
        Assert.Equal(message, groupChat.Message);
        Assert.Equal(status, groupChat.Status);
        Assert.Equal(type, groupChat.MarkedType);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatMessageId>()), Times.Once);
        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateChatMessageAsync_ShouldUpdateChatMessageStatusProperty()
    {
        // Arrange
        GroupChatMessageId id = 1;
        const string message = "original message";
        const MessageStatus status = MessageStatus.Read;
        const MessageMarkedType type = MessageMarkedType.None;

        var groupChatDto = GroupChatMessageTestData.CreateDto(message: message, markedType: type);
        var groupChat = GroupChatMessageTestData.Create(message: message, markedType: type);

        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatMessageId>())).ReturnsAsync(groupChat);
        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, null);

        // Act
        await service.UpdateChatMessageAsync(id, null, status, null);

        // Assert
        Assert.Equal(message, groupChat.Message);
        Assert.Equal(status, groupChat.Status);
        Assert.Equal(type, groupChat.MarkedType);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatMessageId>()), Times.Once);
        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateChatMessageAsync_ShouldUpdateChatMessageMarkedTypeProperty()
    {
        // Arrange
        GroupChatMessageId id = 1;
        const string message = "original message";
        const MessageStatus status = MessageStatus.Sent;
        const MessageMarkedType type = MessageMarkedType.NotReleveant;

        var groupChatDto = GroupChatMessageTestData.CreateDto(message: message, status: status);
        var groupChat = GroupChatMessageTestData.Create(message: message, status: status);

        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatMessageId>())).ReturnsAsync(groupChat);
        mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, null);

        // Act
        await service.UpdateChatMessageAsync(id, null, null, type);

        // Assert
        Assert.Equal(message, groupChat.Message);
        Assert.Equal(status, groupChat.Status);
        Assert.Equal(type, groupChat.MarkedType);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatMessageId>()), Times.Once);
        mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        const int chatId = 1;

        var groupChatsDto = GroupChatMessageTestData.CreateDtoCollection();
        var groupChats = GroupChatMessageTestData.CreateCollection();

        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, null);

        // Act
        await service.DeleteAsync(chatId);

        // Assert and Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<GroupChatMessageId>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowEntityNotFoundException_ShouldNotRemoveEntityAsEntityDoesNotExist()
    {
        // Arrange
        const int chatId = 0;

        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<GroupChatMessageId>())).Throws(new EntityNotFoundException(typeof(GroupChatMessage), chatId));

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, null);

        // Act and Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.DeleteAsync(chatId));

        // Verify correct method calls
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<GroupChatMessageId>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_Collection_ShouldReturnAllEntity()
    {
        // Arrange
        var groupChatsDto = GroupChatMessageTestData.CreateDtoCollection();
        var groupChats = GroupChatMessageTestData.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockMapper.Setup(m => m.Map<IEnumerable<GroupChatMessageDto>>(It.IsAny<IEnumerable<GroupChatMessage>>())).Returns(groupChatsDto);

        mockRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(groupChats);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<GroupChatMessageDto>>(It.IsAny<IEnumerable<GroupChatMessage>>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_GroupChatMessage_ShouldReturnCorrectEntity()
    {
        // Arrange
        const int chatId = 1;

        var groupChatDto = GroupChatMessageTestData.CreateDto();
        var groupChat = GroupChatMessageTestData.Create();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockMapper.Setup(m => m.Map<GroupChatMessageDto>(It.IsAny<GroupChatMessage>())).Returns(groupChatDto);

        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<GroupChatMessageId>())).ReturnsAsync(groupChat);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByIdAsync(chatId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(groupChatDto.Id, result.Id);
        Assert.Equal(groupChatDto.Username, result.Username);
        Assert.Equal(groupChatDto.Message, result.Message);
        Assert.Equal(groupChatDto.Time, result.Time);
        Assert.Equal(groupChatDto.Status, result.Status);
        Assert.Equal(groupChatDto.Type, result.Type);
        Assert.Equal(groupChatDto.MarkedType, result.MarkedType);
        Assert.Equal(groupChatDto.GroupChatId, result.GroupChatId);
        Assert.Equal(groupChatDto.GroupChatUserId, result.GroupChatUserId);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<GroupChatMessageId>()), Times.Once);
        mockMapper.Verify(m => m.Map<GroupChatMessageDto>(It.IsAny<GroupChatMessage>()), Times.Once);
    }

    [Fact]
    public async Task GetByChatIdAsync_Collection_ShouldReturnCollectionByChatId()
    {
        // Arrange
        const int chatId = 1;
        const int page = 1;
        const int pageSize = 2;

        var groupChatsDto = GroupChatMessageTestData.CreateDtoCollection();
        var groupChatsDomainDto = GroupChatMessageTestData.CreateDomainDtoCollection();
        var groupChats = GroupChatMessageTestData.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockMapper.Setup(m => m.Map<IEnumerable<GroupChatMessageDto>>(It.IsAny<IEnumerable<Domain.DTOs.GroupChatMessageDto>>())).Returns(groupChatsDto);

        mockRepository.Setup(m => m.GetByChatIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(groupChatsDomainDto);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByChatIdAsync(chatId, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByChatIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<GroupChatMessageDto>>(It.IsAny<IEnumerable<Domain.DTOs.GroupChatMessageDto>>()), Times.Once);
    }

    [Fact]
    public async Task CountByChatIdAsync_Count_ShouldReturnCountEntityByChatId()
    {
        // Arrange
        const int chatId = 1;
        const int count = 1;

        var groupChatsDto = GroupChatMessageTestData.CreateDtoCollection();
        var groupChats = GroupChatMessageTestData.CreateCollection();

        var mockRepository = new Mock<IGroupChatMessageRepository>();
        var mockChatRepository = new Mock<IGenericRepository<GroupChat, GroupChatId>>();
        var mockChatUserRepository = new Mock<IGenericRepository<GroupChatUser, GroupChatUserId>>();

        mockRepository.Setup(m => m.CountByChatIdAsync(It.IsAny<int>())).ReturnsAsync(count);

        var service = new GroupChatMessageService(mockRepository.Object, mockChatRepository.Object, mockChatUserRepository.Object, null);

        // Act
        var result = await service.CountByChatIdAsync(chatId);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountByChatIdAsync(It.IsAny<int>()), Times.Once);
    }
}
