using Chat.Application.DTOs;
using Chat.Application.Interfaces;

namespace Chat.Application.Services;

public interface IPersonalChatMessageService : IService<PersonalChatMessageDto, int>
{
    Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsync(int chatId, int page, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
