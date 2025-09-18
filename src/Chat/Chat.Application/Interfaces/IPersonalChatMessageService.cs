using Chat.Application.DTOs;
using Chat.Domain.Enums;

namespace Chat.Application.Interfaces;

public interface IPersonalChatMessageService : IService<PersonalChatMessageDto, int>
{
    Task UpdateStatusAsync(int messageId, MessageStatus newStatus);

    Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
