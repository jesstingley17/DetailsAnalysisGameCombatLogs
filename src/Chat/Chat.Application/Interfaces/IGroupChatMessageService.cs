using Chat.Application.DTOs;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Interfaces;

public interface IGroupChatMessageService : IService<GroupChatMessageDto, int>
{
    Task UpdateChatMessageAsync(GroupChatMessageId id,
                                         string? message,
                                         MessageStatus? status,
                                         MessageMarkedType? markedType);

    Task ReadMessagesLessThanAsync(int chatId, int messageId);

    Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountReadUnreadMessagesAsync(int chatId, int chatMessageId, int lastReadMessageId);

    Task<int> CountByChatIdAsync(int chatId);
}