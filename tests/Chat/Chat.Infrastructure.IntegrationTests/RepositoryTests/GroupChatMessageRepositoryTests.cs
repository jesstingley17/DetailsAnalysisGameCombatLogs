using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Repositories;
using Chat.Infrastructure.IntegrationTests.Factory;

namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

public class GroupChatMessageRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetByChatIdAsync_ShouldReturnCollectionOfChatMessages()
    {
        // Arrange
        GroupChatId chatId = 1;
        const int page = 1;
        const int pageSize = 2;

        using var context = CreateInMemoryContext();
        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
        await context.Set<GroupChatMessage>().AddRangeAsync(GroupChatMessageTestData.CreateCollection());
        await context.Set<GroupChatUser>().AddRangeAsync(GroupChatUserTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GroupChatMessageRepository(context);

        // Act
        var result = await repo.GetByChatIdAsync(chatId, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ReadMessagesLessThanAsync_ShouldReadMessagesLessThan()
    {
        // Arrange
        GroupChatId chatId = 1;
        GroupChatMessageId messageId = 1;
        const MessageStatus read = MessageStatus.Read;

        using var context = CreateInMemoryContext();
        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
        await context.Set<GroupChatMessage>().AddRangeAsync(GroupChatMessageTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GroupChatMessageRepository(context);

        // Act
        await repo.ReadMessagesLessThanAsync(chatId, messageId);
        var result = await repo.GetByIdAsync(messageId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(messageId.Value, result.Id.Value);
        Assert.Equal(read, result.Status);
    }

    [Fact]
    public async Task CountReadUnreadMessagesAsync_ShouldCountReadUnreadMessagesUseLastMessageId()
    {
        // Arrange
        GroupChatId chatId = 1;
        GroupChatMessageId messageId = 1;
        GroupChatMessageId lastMessageId = 1;

        using var context = CreateInMemoryContext();
        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
        await context.Set<GroupChatMessage>().AddRangeAsync(GroupChatMessageTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GroupChatMessageRepository(context);

        // Act
        var count = await repo.CountReadUnreadMessagesAsync(chatId, messageId, lastMessageId);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task CountReadUnreadMessagesAsync_ShouldCountReadUnreadMessages()
    {
        // Arrange
        GroupChatId chatId = 1;
        GroupChatMessageId messageId = 1;

        using var context = CreateInMemoryContext();
        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
        await context.Set<GroupChatMessage>().AddRangeAsync(GroupChatMessageTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GroupChatMessageRepository(context);

        // Act
        var count = await repo.CountReadUnreadMessagesAsync(chatId, messageId);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task CountByChatIdAsync_ShouldCountMessagesByChatId()
    {
        // Arrange
        GroupChatId chatId = 1;

        using var context = CreateInMemoryContext();
        await context.Set<GroupChat>().AddRangeAsync(GroupChatTestData.CreateCollection());
        await context.Set<GroupChatMessage>().AddRangeAsync(GroupChatMessageTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new GroupChatMessageRepository(context);

        // Act
        var count = await repo.CountByChatIdAsync(chatId);

        // Assert
        Assert.Equal(1, count);
    }
}
