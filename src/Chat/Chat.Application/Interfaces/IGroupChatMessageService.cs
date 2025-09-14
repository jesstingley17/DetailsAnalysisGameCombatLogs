using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IGroupChatMessageService : IService<GroupChatMessageDto, int>
{
    Task<IEnumerable<GroupChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}