using Chat.Application.DTOs;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Interfaces;

public interface IPersonalChatMessageService : IService<PersonalChatMessageDto, int>
{
    Task UpdateChatMessageAsync(PersonalChatMessageId id,
                                         string? message,
                                         MessageStatus? status,
                                         MessageMarkedType? markedType);

    Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
