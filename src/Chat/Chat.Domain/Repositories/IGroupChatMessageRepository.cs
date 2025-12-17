using Chat.Domain.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatMessageRepository : IGenericRepository<GroupChatMessage, GroupChatMessageId>
{
    Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task ReadMessagesLessThanAsync(int chatId, int messageId);

    Task<int> CountReadUnreadMessagesAsync(int chatId, int chatMessageId, int lastReadMessageId);

    Task<int> CountReadUnreadMessagesAsync(int chatId, int chatMessageId);

    Task<int> CountByChatIdAsync(int chatId);
}
