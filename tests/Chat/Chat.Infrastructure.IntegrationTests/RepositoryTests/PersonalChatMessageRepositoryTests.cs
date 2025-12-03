using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.IntegrationTests.Factory;
using Chat.Infrastructure.Repositories;

namespace Chat.Infrastructure.IntegrationTests.RepositoryTests;

public class PersonalChatMessageRepositoryTests : RepositoryTestsBase
{
    [Fact]
    public async Task GetByChatIdAsync_ShouldReturnCollectionOfChatMessages()
    {
        // Arrange
        PersonalChatId chatId = 1;
        const int page = 1;
        const int pageSize = 2;

        using var context = CreateInMemoryContext();
        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
        await context.Set<PersonalChatMessage>().AddRangeAsync(PersonalChatMessageTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new PersonalChatMessageRepository(context);

        // Act
        var result = await repo.GetByChatIdAsync(chatId, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task CountByChatIdAsync_ShouldCountMessagesByChatId()
    {
        // Arrange
        PersonalChatId chatId = 1;

        using var context = CreateInMemoryContext();
        await context.Set<PersonalChat>().AddRangeAsync(PersonalChatTestData.CreateCollection());
        await context.Set<PersonalChatMessage>().AddRangeAsync(PersonalChatMessageTestData.CreateCollection());
        await context.SaveChangesAsync();

        var repo = new PersonalChatMessageRepository(context);

        // Act
        var count = await repo.CountByChatIdAsync(chatId);

        // Assert
        Assert.Equal(1, count);
    }
}
