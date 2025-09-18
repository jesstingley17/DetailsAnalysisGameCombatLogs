using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IPersonalChatMessageRepository : IGenericRepository<PersonalChatMessage, PersonalChatMessageId>
{
    Task UpdateStatusAsync(int messageId, MessageStatus newStatus);

    Task UpdateAsync(PersonalChatMessage item);

    Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
